using FluentValidation;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Handlers;

namespace zero.Core.Collections
{
  public class PagesCollection : CollectionBase<Page>, IPagesCollection
  {
    const string RECYCLE_BIN_GROUP = "pages";

    protected IRecycleBinApi RecycleBinApi { get; private set; }

    protected ILogger<IPagesCollection> Logger { get; private set; }

    protected IHandlerHolder Handler { get; private set; }


    public PagesCollection(IZeroContext context, ICollectionInterceptorHandler interceptor, IRecycleBinApi recycleBinApi, IHandlerHolder handler,
      ILogger<IPagesCollection> logger, IValidator<Page> validator = null) : base(context, interceptor, validator)
    {
      RecycleBinApi = recycleBinApi;
      Handler = handler;
      Logger = logger;
    }


    /// <inheritdoc />
    public Task<Page> GetEmpty(string pageType, string parentId = null)
    {
      PageType type = GetPageType(pageType);

      if (type == null)
      {
        return Task.FromResult<Page>(null);
      }

      try
      {
        Page model = Activator.CreateInstance(type.ContentType) as Page;

        model.PageTypeAlias = type.Alias;
        model.ParentId = parentId; // TODO validate if type is allowed and if parentid is allowed

        return Task.FromResult(model);
      }
      catch
      {
        Logger.LogWarning("Could not create page with type {type}", type);
      }

      return Task.FromResult<Page>(null);
    }


    /// <inheritdoc />
    public IList<PageType> GetPageTypes() => Context.Options.Pages.GetAllItems().ToList();


    /// <inheritdoc />
    public PageType GetPageType(string alias) => Context.Options.Pages.GetAllItems().FirstOrDefault(x => x.Alias == alias);


    /// <inheritdoc />
    public async Task<IList<PageType>> GetAllowedPageTypes(string parentId = null)
    {
      IEnumerable<PageType> types = Context.Options.Pages.GetAllItems();
      List<Page> parents = new();

      if (!parentId.IsNullOrEmpty())
      {
        Pages_ByHierarchy.Result result = await Session.Query<Pages_ByHierarchy.Result, Pages_ByHierarchy>()
          .ProjectInto<Pages_ByHierarchy.Result>()
          .Include<Pages_ByHierarchy.Result, Page>(x => x.Id)
          .Include<Pages_ByHierarchy.Result, Page>(x => x.Path.Select(p => p.Id))
          .FirstOrDefaultAsync(x => x.Id == parentId);

        if (result != null)
        {
          List<string> ids = result.Path.Select(x => x.Id).ToList();
          ids.Add(result.Id);
          parents = (await Session.LoadAsync<Page>(ids)).Select(x => x.Value).Reverse().ToList();
        }
      }

      IPageTypeHandler handler = Handler.Get<IPageTypeHandler>();

      // if there is no registered handler we just allow all page types
      if (handler == null)
      {
        return types.ToList();
      }

      return handler.GetAllowedPageTypes(Context.Application, types, parents)?.ToList() ?? new();
    }


    /// <inheritdoc />
    public async Task<EntityResult<IList<Page>>> SaveSorting(string[] sortedIds)
    {
      Dictionary<string, Page> items = await GetByIds(sortedIds);
      uint index = 0;

      // contains multiple parents, therefore fail
      if (items.Select(x => x.Value?.ParentId).Distinct().Count() > 1)
      {
        return EntityResult<IList<Page>>.Fail("@errors.page.sortingmultipleparents");
      }

      foreach (var item in items)
      {
        item.Value.Sort = index;
        index += 10;
        await Save(item.Value);
        //session.Advanced.Patch<T, uint>(item.Value.Id, x => x.Sort, index++);
      }

      return EntityResult<IList<Page>>.Success(items.Select(x => x.Value).ToList());
    }


    /// <inheritdoc />
    public async Task<EntityResult<Page>> Move(string id, string parentId)
    {
      Page model = await GetById(id);
      Page parent = await GetById(parentId);

      if (model == null || (!parentId.IsNullOrEmpty() && parent == null))
      {
        return EntityResult<Page>.Fail("@errors.idnotfound");
      }

      IList<PageType> pageTypes = await GetAllowedPageTypes(parentId);

      if (!pageTypes.Any(x => x.Alias == model.PageTypeAlias))
      {
        return EntityResult<Page>.Fail("@errors.page.parentnotallowed");
      }

      model.ParentId = parent?.Id;

      return await Save(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Page>> Copy(string id, string destinationId, bool includeDescendants = false)
    {
      Page model = await GetById(id);
      Page parent = await GetById(destinationId);

      if (model == null || (!destinationId.IsNullOrEmpty() && parent == null))
      {
        return EntityResult<Page>.Fail("@errors.idnotfound");
      }

      string baseId = model.Id;

      // update new page properties
      model.Id = null;
      model.ParentId = parent?.Id;
      model.IsActive = false;
      model.CreatedDate = DateTimeOffset.Now;

      // check if new parent is allowed
      IList<PageType> pageTypes = await GetAllowedPageTypes(destinationId);

      if (!pageTypes.Any(x => x.Alias == model.PageTypeAlias))
      {
        return EntityResult<Page>.Fail("@errors.page.parentnotallowed");
      }

      // recursive function to store descendants
      async Task AddChildren(string oldParentId, string newParentId)
      {
        Pages_WithChildren.Result childrenResult = await Session.Query<Pages_WithChildren.Result, Pages_WithChildren>()
          .ProjectInto<Pages_WithChildren.Result>()
          .Include<Pages_WithChildren.Result, Page>(x => x.Id)
          .Where(x => x.Id == oldParentId)
          .FirstOrDefaultAsync();

        if (childrenResult == null || childrenResult.ChildrenIds.Length < 1)
        {
          return;
        }

        Dictionary<string, Page> childrenPages = await GetByIds(childrenResult.ChildrenIds);

        foreach (var child in childrenPages)
        {
          Page childPage = child.Value.Clone();
          childPage.Id = null;
          childPage.IsActive = false;
          childPage.ParentId = newParentId;
          childPage.CreatedDate = DateTimeOffset.Now;

          await Save(childPage);
          await AddChildren(child.Key, childPage.Id);
        }
      }

      if (includeDescendants)
      {
        await AddChildren(baseId, model.Id);
      }

      return EntityResult<Page>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<string[]>> Delete(string id, bool recursive = true, bool moveToRecycleBin = true)
    {
      List<Page> pages = recursive ? await GetByIdWithDescendants(id) : new() { await GetById(id) };

      if (pages.Count < 1)
      {
        return EntityResult<string[]>.Fail("@errors.ondelete.idnotfound");
      }

      if (moveToRecycleBin)
      {
        await RecycleBinApi.Add(pages, RECYCLE_BIN_GROUP);
      }

      await Delete(pages.ToArray());

      return EntityResult<string[]>.Success(pages.Select(x => x.Id).ToArray());
    }


    /// <inheritdoc />
    public async Task<EntityResult<string[]>> Restore(string id, bool includeDescendants = false)
    {
      EntityResult<string[]> result = new EntityResult<string[]>();
      RecycledEntity recycledEntity = await RecycleBinApi.GetById(id);
      List<RecycledEntity> entities = new List<RecycledEntity>() { recycledEntity };

      if (recycledEntity == null)
      {
        return EntityResult<string[]>.Fail(); // TODO correct error message
      }

      // get descendants from the operation
      if (includeDescendants && !recycledEntity.OperationId.IsNullOrEmpty())
      {
        entities = (await RecycleBinApi.GetByOperation(recycledEntity.OperationId)).ToList();
      }

      // fill ids
      string[] ids = entities.Select(x => x.OriginalId).ToArray();

      // check if parents are available
      string[] parentIds = entities.Select(x => x.Content as Page).Where(x => x != null).Select(x => x.ParentId).ToArray();
      parentIds = (await GetByIds(parentIds)).Where(x => x.Value != null).Select(x => x.Value.Id).ToArray();

      // validate and restore all items
      foreach (RecycledEntity entity in entities)
      {
        // check if it contains page data
        if (entity.Group != RECYCLE_BIN_GROUP || !(entity.Content is Page))
        {
          //result.AddError("Cannot parse recycled entity as an Page in group \"" + RECYCLE_BIN_GROUP + "\""); // TODO correct error message
          continue;
        }

        // get page
        Page page = entity.Content as Page;
        page.IsActive = false;

        // validate app and parent
        if (!page.ParentId.IsNullOrEmpty() && !ids.Contains(page.ParentId) && !parentIds.Contains(page.ParentId))
        {
          // TODO correct error message
          continue;
        }

        // restore to pages
        EntityResult<Page> saveResult = await Save(page);
      }

      // delete affected entities from recycle bin
      if (!recycledEntity.OperationId.IsNullOrEmpty())
      {
        await RecycleBinApi.DeleteByOperation(recycledEntity.OperationId);
      }

      // set result
      result.Model = ids;
      result.IsSuccess = true;

      return result;
    }


    /// <summary>
    /// Get a page with all its descendants
    /// </summary>
    async Task<List<Page>> GetByIdWithDescendants(string id)
    {
      List<Page> items = new List<Page>();

      Page model = await GetById(id);

      if (model == null)
      {
        return items;
      }

      items.Add(model);

      // recursive function to store descendants
      async Task AddChildren(string parentId)
      {
        Pages_WithChildren.Result childrenResult = await Session.Query<Pages_WithChildren.Result, Pages_WithChildren>()
          .ProjectInto<Pages_WithChildren.Result>()
          .Include<Pages_WithChildren.Result, Page>(x => x.Id)
          .Where(x => x.Id == parentId)
          .FirstOrDefaultAsync();

        if (childrenResult == null || childrenResult.ChildrenIds.Length < 1)
        {
          return;
        }

        Dictionary<string, Page> childrenPages = await GetByIds(childrenResult.ChildrenIds);

        foreach (var child in childrenPages)
        {
          items.Add(child.Value);
          await AddChildren(child.Value.Id);
        }
      }

      await AddChildren(model.Id);

      return items;
    }
  }


  public interface IPagesCollection : ICollectionBase<Page>
  {
    /// <summary>
    /// Get a new empty page with the specified type
    /// </summary>
    public Task<Page> GetEmpty(string pageType, string parentId = null);

    /// <summary>
    /// Get all available page types
    /// </summary>
    IList<PageType> GetPageTypes();

    /// <summary>
    /// Get all page types which are allowed below a selected parent page
    /// </summary>
    Task<IList<PageType>> GetAllowedPageTypes(string parentId = null);

    /// <summary>
    /// Get a specific page type by alias
    /// </summary>
    PageType GetPageType(string alias);

    /// <summary>
    /// Update sorting of pages on a specific level
    /// </summary>
    Task<EntityResult<IList<Page>>> SaveSorting(string[] sortedIds);

    /// <summary>
    /// Move a page to a new parent
    /// </summary>
    Task<EntityResult<Page>> Move(string id, string parentId);

    /// <summary>
    /// Copies a page (with optional descendants) to a new location
    /// </summary>
    Task<EntityResult<Page>> Copy(string id, string destinationId, bool includeDescendants = false);

    /// <summary>
    /// Deletes a page by Id (with all it's descendants)
    /// </summary>
    Task<EntityResult<string[]>> Delete(string id, bool recursive = true, bool moveToRecycleBin = true);

    /// <summary>
    /// Restores a page from the recycle bin
    /// </summary>
    Task<EntityResult<string[]>> Restore(string id, bool includeDescendants = false);
  }
}
