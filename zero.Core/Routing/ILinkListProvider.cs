using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface ILinkListProvider : ILinkProvider
  {
    /// <summary>
    /// Get paged list items.
    /// <param name="session">Current document session</param>
    /// <param name="page">Current page number (one-based)</param>
    /// <param name="search">Search query</param>
    /// </summary>
    Task<IList<TreeItem>> GetLinkListItems(IAsyncDocumentSession session, int page = 1, string search = null);
  }
}
