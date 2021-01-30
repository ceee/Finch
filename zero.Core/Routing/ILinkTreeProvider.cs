using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface ILinkTreeProvider : ILinkProvider
  {
    /// <summary>
    /// Get tree children for the current parent id.
    /// <param name="session">Current document session</param>
    /// <param name="parentId">Parent node id</param>
    /// <param name="activeId">Selected node so parents can be set to open for the tree to load correclty</param>
    /// </summary>
    Task<IList<TreeItem>> GetLinkTreeItems(IAsyncDocumentSession session, string parentId = null, string activeId = null);
  }
}
