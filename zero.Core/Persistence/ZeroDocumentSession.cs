using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace zero.Persistence;

public class ZeroDocumentSession : AsyncDocumentSession, IZeroDocumentSession
{
  public IZeroDocumentSession Core { get; private set; }

  public IDocumentSession Synchronous => _synchronous ?? (_synchronous = DocumentStore.OpenSession(DatabaseName));

  IDocumentSession _synchronous;

  public ZeroDocumentSession(DocumentStore documentStore, Guid id, SessionOptions options, string coreDatabase = null) : base(documentStore, id, options)
  {
    if (coreDatabase.HasValue())
    {
      Core = new ZeroDocumentSession(documentStore, id, new SessionOptions()
      {
        Database = coreDatabase,
        DisableAtomicDocumentWritesInClusterWideTransaction = options.DisableAtomicDocumentWritesInClusterWideTransaction,
        NoCaching = options.NoCaching,
        NoTracking = options.NoTracking,
        RequestExecutor = options.RequestExecutor,
        TransactionMode = options.TransactionMode
      });
    }
    else
    {
      Core = this;
    }
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
  IZeroDocumentSession Core { get; }

  IDocumentSession Synchronous { get; }

  bool IsDisposed { get; }
}
