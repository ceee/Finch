using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace zero.Persistence;

public class ZeroDocumentSession : AsyncDocumentSession, IZeroDocumentSession
{
  public IZeroDocumentSession Core { get; private set; }

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
}


public interface IZeroDocumentSession : IAsyncDocumentSession
{
  IZeroDocumentSession Core { get; }

  bool IsDisposed { get; }
}
