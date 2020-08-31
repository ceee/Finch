using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class SpacesApi : AppAwareBackofficeApi, ISpacesApi
  {
    protected IPermissionsApi PermissionsApi { get; private set; }


    public SpacesApi(IBackofficeStore store, IPermissionsApi permissionsApi) : base(store)
    {
      Scope.IncludeShared = true;
      PermissionsApi = permissionsApi;
    }


    /// <inheritdoc />
    public Space GetByAlias(string alias)
    {
      return GetAll().FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public Space GetBy<T>() where T : ISpaceContent
    {
      Type type = typeof(T);
      return GetAll().FirstOrDefault(x => x.Type == type);
    }


    /// <inheritdoc />
    public IReadOnlyCollection<Space> GetAll()
    {
      return Backoffice.Options.Spaces.GetAllItems();
    }


    /// <inheritdoc />
    public async Task<ISpaceContent> GetItem(string alias, string id = null)
    {
      Space space = GetByAlias(alias);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<ISpaceContent>()
          .Scope(Scope)
          .Where(x => x.SpaceAlias == space.Alias)
          .WhereIf(x => x.Id == id, !id.IsNullOrEmpty())
          .FirstOrDefaultAsync();
      }
    }


    /// <inheritdoc />
    public async Task<T> GetItem<T>(string id = null) where T : ISpaceContent
    {
      Space space = GetBy<T>();

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .Scope(Scope)
          .Where(x => x.SpaceAlias == space.Alias)
          .WhereIf(x => x.Id == id, !id.IsNullOrEmpty())
          .FirstOrDefaultAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<ISpaceContent>> GetListByQuery(string alias, ListQuery<ISpaceContent> query)
    {
      query.SearchSelector = item => item.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<ISpaceContent>()
          .Scope(Scope)
          .Where(x => x.SpaceAlias == alias)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetListByQuery<T>(ListQuery<T> query) where T : ISpaceContent
    {
      Space space = GetBy<T>();
      query.SearchSelector = item => item.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .Scope(Scope)
          .Where(x => x.SpaceAlias == space.Alias)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<ISpaceContent>> Save(ISpaceContent model)
    {
      return await SaveModel(model, null);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ISpaceContent>> Delete(string id)
    {
      return await DeleteById<ISpaceContent>(id);
    }
  }


  public interface ISpacesApi
  {
    /// <summary>
    /// Returns a space by the defined alias
    /// </summary>
    Space GetByAlias(string alias);

    /// <summary>
    /// Returns a space by a defined generic
    /// </summary>
    Space GetBy<T>() where T : ISpaceContent;

    /// <summary>
    /// Get all spaces
    /// </summary>
    IReadOnlyCollection<Space> GetAll();

    /// <summary>
    /// Get editor item for a space
    /// </summary>
    Task<ISpaceContent> GetItem(string alias, string id = null);

    /// <summary>
    /// Get editor item for a space
    /// </summary>
    Task<T> GetItem<T>(string id = null) where T : ISpaceContent;

    /// <summary>
    /// Get all list items for a space (with query)
    /// </summary>
    Task<ListResult<ISpaceContent>> GetListByQuery(string alias, ListQuery<ISpaceContent> query);

    /// <summary>
    /// Get all list items for a space (with query)
    /// </summary>
    Task<ListResult<T>> GetListByQuery<T>(ListQuery<T> query) where T : ISpaceContent;

    /// <summary>
    /// Saves a content item in a space
    /// </summary>
    Task<EntityResult<ISpaceContent>> Save(ISpaceContent model);

    /// <summary>
    /// Deletes a space content item
    /// </summary>
    Task<EntityResult<ISpaceContent>> Delete(string id);
  }
}
