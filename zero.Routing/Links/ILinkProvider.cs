using Raven.Client.Documents.Session;

namespace zero.Routing;

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
  Task<LinkPreview> Preview(IAsyncDocumentSession session, Link link);
}