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
    bool CanProcess(ILink link);

    /// <summary>
    /// 
    /// </summary>
    Task<string> Resolve(ILink link);

    /// <summary>
    /// 
    /// </summary>
    Task<PreviewModel> Preview(IAsyncDocumentSession session, ILink link);
  }
}
