using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using System;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Options;

namespace zero.Core.Database
{
  public class ZeroStore : DocumentStore, IZeroStore
  {
    public ZeroStore(IZeroOptions options) : base()
    {
      Options = options;
      Database = null;
    }

    protected IZeroOptions Options { get; set; }

    /// <inheritdoc />
    public string ResolvedDatabase { get; set; }


    /// <inheritdoc />
    public IDocumentStore Raven => this;


    /// <inheritdoc />
    public OperationExecutor GetOperationExecutor(string database = null)
    {
      return Operations.ForDatabase(database ?? ResolvedDatabase);
    }

    /// <inheritdoc />
    public IAsyncDocumentSession OpenCoreSession()
    {
      return OpenAsyncSession(Options.Raven.Database);
    }

    /// <inheritdoc />
    public override IAsyncDocumentSession OpenAsyncSession(string database)
    {
      return OpenAsyncSession(new SessionOptions()
      {
        Database = database
      });
    }

    /// <inheritdoc />
    public override IAsyncDocumentSession OpenAsyncSession(SessionOptions options)
    {
      options.Database = options.Database ?? ResolvedDatabase;

      AssertInitialized();
      EnsureNotClosed();

      var sessionId = Guid.NewGuid();
      var session = new ZeroDocumentSession(this, sessionId, options);
      RegisterEvents(session);
      AfterSessionCreated(session);

      return session;
    }

    /// <inheritdoc />
    public override IAsyncDocumentSession OpenAsyncSession()
    {
      return OpenAsyncSession(new SessionOptions()
      {
        Database = ResolvedDatabase
      });
    }

    /// <inheritdoc />
    public override IDocumentSession OpenSession(SessionOptions options)
    {
      options.Database = options.Database ?? ResolvedDatabase;

      AssertInitialized();
      EnsureNotClosed();

      var sessionId = Guid.NewGuid();
      var session = new DocumentSession(this, sessionId, options);
      RegisterEvents(session);
      AfterSessionCreated(session);

      return session;
    }

    /// <inheritdoc />
    public override IDocumentSession OpenSession(string database)
    {
      return OpenSession(new SessionOptions()
      {
        Database = database
      });
    }

    /// <inheritdoc />
    public override IDocumentSession OpenSession()
    {
      return OpenSession(new SessionOptions()
      {
        Database = ResolvedDatabase
      });
    }

    /// <inheritdoc />
    public override BulkInsertOperation BulkInsert(string database = null, CancellationToken token = default)
    {
      return base.BulkInsert(database ?? ResolvedDatabase, token);
    }

    /// <inheritdoc />
    public async Task PurgeAsync<T>(string database = null, string querySuffix = null, Parameters parameters = null)
    {
      var collectionName = Conventions.FindCollectionName(typeof(T));
      var operationQuery = new DeleteByQueryOperation(new IndexQuery()
      {
        Query = $"from {collectionName} c {querySuffix ?? String.Empty}",
        QueryParameters = parameters
      }, new QueryOperationOptions { AllowStale = true });

      Operation operation = await GetOperationExecutor(database).SendAsync(operationQuery);

      await operation.WaitForCompletionAsync();
    }
  }


  public interface IZeroStore : IDocumentStore
  {
    /// <summary>
    /// The database which has been resolved from the current application
    /// </summary>
    string ResolvedDatabase { get; set; }

    /// <summary>
    /// Get underlying raven document store
    /// </summary>
    IDocumentStore Raven { get; }

    /// <summary>
    /// Get operation executor
    /// </summary>
    OperationExecutor GetOperationExecutor(string database = null);

    /// <summary>
    /// Purges a collection
    /// </summary>
    Task PurgeAsync<T>(string database = null, string querySuffix = null, Parameters parameters = null);

    /// <summary>
    /// Opens a session for the core database
    /// </summary>
    IAsyncDocumentSession OpenCoreSession();
  }
}
