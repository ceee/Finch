using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class PagesApi<T> : AppAwareBackofficeApi, IPagesApi<T> where T : IPage
  {
    const string RECYCLE_BIN_GROUP = "pages";

    protected IZeroOptions Options { get; private set; }

    protected IRecycleBinApi RecycleBinApi { get; private set; }
    

    public PagesApi(IZeroOptions options, IBackofficeStore store, IRecycleBinApi recycleBinApi) : base(store)
    {
      Options = options;
      RecycleBinApi = recycleBinApi;
    }


    /// <inheritdoc />
    public async Task<T> GetById(string id)
    {
      var res = await GetById<IPage>(id); // this works
      return await GetById<T>(id);
    }


    /// <inheritdoc />
    public IList<PageType> GetPageTypes()
    {
      return Options.Pages.GetAllItems().ToList();
    }


    /// <inheritdoc />
    public async Task<IList<PageType>> GetAllowedPageTypes(string parentId = null)
    {    
      IEnumerable<PageType> types = Options.Pages.GetAllItems();

      if (parentId.IsNullOrEmpty())
      {
        return types.Where(x => x.AllowAsRoot || x.OnlyAtRoot).ToList();
      }

      Page page = await GetById<Page>(parentId);
      PageType pageType = page != null ? types.FirstOrDefault(x => x.Alias == page.PageTypeAlias) : null;

      if (pageType == null)
      {
        return new List<PageType>();
      }

      if (pageType.AllowAllChildrenTypes)
      {
        return types.Where(x => !x.OnlyAtRoot).ToList();
      }

      return types.Where(x => !x.OnlyAtRoot && pageType.AllowedChildrenTypes.Contains(x.Alias)).ToList();
    }


    /// <inheritdoc />
    public PageType GetPageType(string alias)
    {
      return Options.Pages.GetAllItems().FirstOrDefault(x => x.Alias == alias);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      return await SaveModel(model, null);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> SaveSorting(string[] sortedIds)
    {
      Dictionary<string, T> items = await GetByIds<T>(sortedIds);
      uint index = 0;

      // TODO check if all items are valid

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

        foreach (var item in items)
        {
          item.Value.Sort = index;
          index += 23;
          await session.StoreAsync(item.Value);
          //session.Advanced.Patch<T, uint>(item.Value.Id, x => x.Sort, index++);
        }

        await session.SaveChangesAsync();
      }

      return EntityResult<T>.Success();
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Move(string id, string parentId)
    {
      T model = await GetById<T>(id);
      model.ParentId = parentId;
      return await Save(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Copy(string id, string destinationId, bool includeDescendants = false)
    {
      T model = await GetById<T>(id);

      string baseId = model.Id;

      // update new page properties
      model.Id = null;
      model.ParentId = destinationId;
      model.IsActive = false;
      model.CreatedDate = DateTimeOffset.Now;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);

        // recursive function to store descendants
        async Task AddChildren(string oldParentId, string newParentId)
        {
          Pages_WithChildren.Result childrenResult = await session.Query<Pages_WithChildren.Result, Pages_WithChildren>()
            .ProjectInto<Pages_WithChildren.Result>()
            .Include<Pages_WithChildren.Result, T>(x => x.Id)
            .Scope(Scope)
            .Where(x => x.Id == oldParentId)
            .FirstOrDefaultAsync();

          if (childrenResult == null || childrenResult.ChildrenIds.Length < 1)
          {
            return;
          }

          Dictionary<string, T> childrenPages = await session.LoadAsync<T>(childrenResult.ChildrenIds);

          foreach (var child in childrenPages)
          {
            T childPage = child.Value.Clone();
            childPage.Id = null;
            childPage.IsActive = false;
            childPage.ParentId = newParentId;
            childPage.CreatedDate = DateTimeOffset.Now;

            await session.StoreAsync(childPage);
            await AddChildren(child.Key, childPage.Id);
          }
        }

        await session.StoreAsync(model);

        if (includeDescendants)
        {
          await AddChildren(baseId, model.Id);
        }

        await session.SaveChangesAsync();
      }

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<string[]>> Delete(string id, bool moveToRecycleBin = true)
    {
      IList<T> pages = await GetByIdWithDescendants(id);
      string[] ids = pages.Select(x => x.Id).ToArray();

      if (moveToRecycleBin)
      {
        await RecycleBinApi.Add(pages, RECYCLE_BIN_GROUP);
      }

      await DeleteByIds<T>(ids);

      return EntityResult<string[]>.Success(ids);
    }


    /// <summary>
    /// Restores a page from the recycle bin
    /// </summary>
    public async Task<EntityResult<string[]>> Restore(string id, bool includeDescendants = false)
    {
      EntityResult<string[]> result = new EntityResult<string[]>();
      IRecycledEntity recycledEntity = await RecycleBinApi.GetById(id);
      List<IRecycledEntity> entities = new List<IRecycledEntity>() { recycledEntity };

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
      string[] parentIds = entities.Select(x => x.Content as IPage).Where(x => x != null).Select(x => x.ParentId).ToArray();
      parentIds = (await GetByIds<IPage>(parentIds)).Where(x => x.Value != null).Select(x => x.Value.Id).ToArray();

      // validate and restore all items
      foreach (IRecycledEntity entity in entities)
      {
        // check if it contains page data
        if (entity.Group != RECYCLE_BIN_GROUP || !(entity.Content is IPage))
        {
          //result.AddError("Cannot parse recycled entity as an IPage in group \"" + RECYCLE_BIN_GROUP + "\""); // TODO correct error message
          continue;
        }

        // get page
        IPage page = entity.Content as IPage;
        page.IsActive = false;

        // validate app and parent
        if (!Scope.IsAllowed(page.AppId) || (!page.ParentId.IsNullOrEmpty() && !ids.Contains(page.ParentId) && !parentIds.Contains(page.ParentId)))
        {
          // TODO correct error message
          continue;
        }

        // restore to pages
        EntityResult<IPage> saveResult = await SaveModel(page);
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
    async Task<List<T>> GetByIdWithDescendants(string id)
    {
      List<T> items = new List<T>();

      T model = await GetById<T>(id);

      if (model == null)
      {
        return items;
      }

      items.Add(model);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        // recursive function to store descendants
        async Task AddChildren(string parentId)
        {
          Pages_WithChildren.Result childrenResult = await session.Query<Pages_WithChildren.Result, Pages_WithChildren>()
            .ProjectInto<Pages_WithChildren.Result>()
            .Include<Pages_WithChildren.Result, T>(x => x.Id)
            .Scope(Scope)
            .Where(x => x.Id == parentId)
            .FirstOrDefaultAsync();

          if (childrenResult == null || childrenResult.ChildrenIds.Length < 1)
          {
            return;
          }

          Dictionary<string, T> childrenPages = await session.LoadAsync<T>(childrenResult.ChildrenIds);

          foreach (var child in childrenPages)
          {
            items.Add(child.Value);
            await AddChildren(child.Value.Id);
          }
        }

        await AddChildren(model.Id);
      }

      return items;
    }
  }


  public interface IPagesApi<T> where T : IPage
  {
    /// <summary>
    /// Get page by Id
    /// </summary>
    Task<T> GetById(string id);

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
    /// Creates or updates a page
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Update sorting of pages on a specific level
    /// </summary>
    Task<EntityResult<T>> SaveSorting(string[] sortedIds);

    /// <summary>
    /// Move a page to a new parent
    /// </summary>
    Task<EntityResult<T>> Move(string id, string parentId);

    /// <summary>
    /// Copies a page (with optional descendants) to a new location
    /// </summary>
    Task<EntityResult<T>> Copy(string id, string destinationId, bool includeDescendants = false);

    /// <summary>
    /// Deletes a page by Id (with all it's descendants)
    /// </summary>
    Task<EntityResult<string[]>> Delete(string id, bool moveToRecycleBin = true);

    /// <summary>
    /// Restores a page from the recycle bin
    /// </summary>
    Task<EntityResult<string[]>> Restore(string id, bool includeDescendants = false);
  }
}
