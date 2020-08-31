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
    public async Task<EntityResult<ISpaceContent>> Save(string alias, ISpaceContent model)
    {
      model.SpaceAlias = GetByAlias(alias)?.Alias;
      return await SaveModel(model, null);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ISpaceContent>> Delete(string alias, string id)
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
    /// Get all spaces
    /// </summary>
    IReadOnlyCollection<Space> GetAll();

    /// <summary>
    /// Get editor item for a space
    /// </summary>
    Task<ISpaceContent> GetItem(string alias, string id = null);

    /// <summary>
    /// Get all list items for a space (with query)
    /// </summary>
    Task<ListResult<ISpaceContent>> GetListByQuery(string alias, ListQuery<ISpaceContent> query);

    /// <summary>
    /// Saves a content item in a space
    /// </summary>
    Task<EntityResult<ISpaceContent>> Save(string alias, ISpaceContent model);

    /// <summary>
    /// Deletes a space content item
    /// </summary>
    Task<EntityResult<ISpaceContent>> Delete(string alias, string id);
  }
}
