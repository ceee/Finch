using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public interface ILinkProvider
  {
    /// <summary>
    /// 
    /// </summary>
    bool CanProcess(Link link);

    /// <summary>
    /// 
    /// </summary>
    Task<string> Resolve(Link link);

    /// <summary>
    /// 
    /// </summary>
    Task<PreviewModel> Preview(IAsyncDocumentSession session, Link link);
  }
}
