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
  public class ListsApi : IListsApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected ZeroOptions Options { get; private set; }


    public ListsApi(IDocumentStore raven, IOptionsMonitor<ZeroOptions> options)
    {
      Raven = raven;
      Options = options.CurrentValue;
    }


    /// <inheritdoc />
    public ListCollection GetCollectionByAlias(string alias)
    {
      return Options.Lists.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public ListCollections GetCollections()
    {
      return Options.Lists;
    }


    /// <inheritdoc />
    public async Task<IList<T>> GetAll<T>(string alias) where T : ListItem
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<ListItem>().ProjectInto<T>().ToListAsync();
      }
    }

    /// <inheritdoc />
    public async Task<ListResult<T>> GetByQuery<T>(string alias, ListQuery<T> query, string appId = null) where T : ListItem
    {
      query.SearchSelector = user => user.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .ForApp(appId)
          .Where(x => x.Alias == alias)
          .ToQueriedListAsync(query);
      }
    }
  }


  public interface IListsApi
  {
    /// <summary>
    /// Returns a collection by the defined alias
    /// </summary>
    ListCollection GetCollectionByAlias(string alias);

    /// <summary>
    /// Get all list collections
    /// </summary>
    ListCollections GetCollections();

    /// <summary>
    /// Get all list items by a list collection alias
    /// </summary>
    Task<IList<T>> GetAll<T>(string alias) where T : ListItem;

    /// <summary>
    /// Get all list items for a collection (with query)
    /// </summary>
    Task<ListResult<T>> GetByQuery<T>(string alias, ListQuery<T> query, string appId = null) where T : ListItem;
  }
}
