using Microsoft.Extensions.Options;
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
  public class SpacesApi : ISpacesApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected ZeroOptions Options { get; private set; }


    public SpacesApi(IDocumentStore raven, IOptionsMonitor<ZeroOptions> options)
    {
      Raven = raven;
      Options = options.CurrentValue;
    }


    /// <inheritdoc />
    public Space GetByAlias(string alias)
    {
      return Options.Spaces.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public SpaceCollection GetAll()
    {
      return Options.Spaces;
    }


    /// <inheritdoc />
    //public async Task<IList<T>> GetAll<T>(string alias) where T : SpaceListItem
    //{
    //  using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
    //  {
    //    return await session.Query<SpaceListItem>().ProjectInto<T>().ToListAsync();
    //  }
    //}

    ///// <inheritdoc />
    //public async Task<ListResult<T>> GetByQuery<T>(string alias, ListQuery<T> query, string appId = null) where T : SpaceListItem
    //{
    //  query.SearchSelector = user => user.Name;

    //  using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
    //  {
    //    return await session.Query<T>()
    //      .ForApp(appId)
    //      .Where(x => x.Alias == alias)
    //      .ToQueriedListAsync(query);
    //  }
    //}
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
    SpaceCollection GetAll();

    /// <summary>
    /// Get all list items by a list collection alias
    /// </summary>
    //Task<IList<T>> GetAll<T>(string alias) where T : SpaceListItem;

    /// <summary>
    /// Get all list items for a collection (with query)
    /// </summary>
    //Task<ListResult<T>> GetByQuery<T>(string alias, ListQuery<T> query, string appId = null) where T : SpaceListItem;
  }
}
