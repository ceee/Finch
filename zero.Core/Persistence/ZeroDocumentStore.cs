using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;

namespace zero.Persistence;

public class ZeroDocumentStore : DocumentStore, IZeroDocumentStore
{
  public ZeroDocumentStore(IZeroOptions options) : base()
  {
    Options = options.For<RavenOptions>();
    Database = null;
  }

  protected RavenOptions Options { get; set; }

  /// <inheritdoc />
  public string ResolvedDatabase { get; set; }


  /// <inheritdoc />
  public OperationExecutor GetOperationExecutor(string database = null)
  {
    return Operations.ForDatabase(database ?? ResolvedDatabase);
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
    var session = new ZeroDocumentSession(this, sessionId, options, Options.Database);
    RegisterEvents(session);
    AfterSessionCreated(session);
    session.OnSessionDisposing += (sender, args) =>
    {
      session.IsDisposed = true;
    };

    session.Advanced.WaitForIndexesAfterSaveChanges(TimeSpan.FromSeconds(3), throwOnTimeout: false);
    session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromHours(1), options.Database); // TODO I guess this will not work in backoffice when we want to see changes immediately
    // maybe use caching for frontend but not for backoffice?

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

    session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromHours(1), options.Database);

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

    Operation operation = await GetOperationExecutor(database ?? ResolvedDatabase).SendAsync(operationQuery);

    await operation.WaitForCompletionAsync();
  }
}


public interface IZeroDocumentStore : IDocumentStore
{
  /// <summary>
  /// The database which has been resolved from the current application
  /// </summary>
  string ResolvedDatabase { get; set; }

  /// <summary>
  /// Get operation executor
  /// </summary>
  OperationExecutor GetOperationExecutor(string database = null);

  /// <summary>
  /// Purges a collection
  /// </summary>
  Task PurgeAsync<T>(string database = null, string querySuffix = null, Parameters parameters = null);
}