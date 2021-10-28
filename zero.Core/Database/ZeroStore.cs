using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
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
    protected Dictionary<string, IZeroDocumentSession> ScopedSessions { get; set; } = new();

    /// <inheritdoc />
    public string ResolvedDatabase { get; set; }


    /// <inheritdoc />
    public IDocumentStore Raven => this;


    /// <inheritdoc />
    public IZeroDocumentSession Session(string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null)
    {
      database ??= ResolvedDatabase;
      options ??= new SessionOptions() { Database = database };

      if (resolution == ZeroSessionResolution.Create)
      {
        return OpenAsyncSession(options) as IZeroDocumentSession;
      }

      if (!ScopedSessions.TryGetValue(database, out IZeroDocumentSession session))
      {
        session = OpenAsyncSession(options) as IZeroDocumentSession;
        ScopedSessions.TryAdd(database, session);
      }
      
      if (session.IsDisposed)
      {
        session = OpenAsyncSession(options) as IZeroDocumentSession;
        ScopedSessions.Remove(database);
        ScopedSessions.TryAdd(database, session);
      }

      return session;
    }


    /// <inheritdoc />
    public IZeroDocumentSession Session(bool global, string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null)
    {
      return Session(global ? Options.Raven.Database : database, resolution, options);
    }


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
      var session = new ZeroDocumentSession(this, sessionId, options, Options.Raven.Database);
      RegisterEvents(session);
      AfterSessionCreated(session);
      session.OnSessionDisposing += (sender, args) =>
      {
        session.IsDisposed = true;
      };

      session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromHours(1), options.Database);

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


  public enum ZeroSessionResolution
  {
    Reuse = 0,
    Create = 1
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
    /// Use a specific session
    /// </summary>
    IZeroDocumentSession Session(string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null);

    /// <summary>
    /// Use a session for the core database
    /// </summary>
    IZeroDocumentSession Session(bool global, string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null);

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
