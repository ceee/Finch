using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

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
  }


  public interface IListsApi
  {
    /// <summary>
    /// Get all list collections
    /// </summary>
    ListCollections GetCollections();

    /// <summary>
    /// Get all list items by a list collection alias
    /// </summary>
    Task<IList<T>> GetAll<T>(string alias) where T : ListItem;
  }
}
