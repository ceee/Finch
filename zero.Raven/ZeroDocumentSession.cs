using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace zero.Raven;

public class ZeroDocumentSession : AsyncDocumentSession, IZeroDocumentSession
{
  public IDocumentSession Synchronous => _synchronous ?? (_synchronous = DocumentStore.OpenSession(DatabaseName));

  IDocumentSession _synchronous;

  public ZeroDocumentSession(DocumentStore documentStore, Guid id, SessionOptions options, string coreDatabase = null) :
    base(documentStore, id, options)
  {
  }

  public bool IsDisposed { get; set; }

  public override void Dispose()
  {
    _synchronous?.Dispose();
    base.Dispose();
  }
}


public interface IZeroDocumentSession : IAsyncDocumentSession
{
  IDocumentSession Synchronous { get; }

  bool IsDisposed { get; }
}
