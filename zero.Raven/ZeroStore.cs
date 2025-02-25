using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Raven.Client;

namespace zero.Raven;

public class ZeroStore : IZeroStore
{
  public ZeroStore(IDocumentStore raven, IZeroOptions options) : base()
  {
    Options = options;
    Raven = raven;
    //Database = null;
  }

  protected IZeroOptions Options { get; set; }
  protected Dictionary<string, IZeroDocumentSession> ScopedSessions { get; set; } = new();
  private const string NullDb = "__default__";


  /// <inheritdoc />
  public IDocumentStore Raven { get; private set; }


  /// <inheritdoc />
  public IZeroDocumentSession Session(string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null)
  {
    string databaseKey = database ?? NullDb;
    
    options ??= new SessionOptions();

    if (database.HasValue())
    {
      options.Database = database;
    }

    if (resolution == ZeroSessionResolution.Create)
    {
      return Raven.OpenAsyncSession(options) as IZeroDocumentSession;
    }

    if (!ScopedSessions.TryGetValue(databaseKey, out IZeroDocumentSession session))
    {
      session = Raven.OpenAsyncSession(options) as IZeroDocumentSession;
      ScopedSessions.TryAdd(databaseKey, session);
    }
      
    if (session.IsDisposed)
    {
      session = Raven.OpenAsyncSession(options) as IZeroDocumentSession;
      ScopedSessions.Remove(databaseKey);
      ScopedSessions.TryAdd(databaseKey, session);
    }

    return session;
  }


  /// <inheritdoc />
  public async Task PurgeAsync<T>(string querySuffix = null, Parameters parameters = null)
  {
    string collectionName = Raven.Conventions.FindCollectionName(typeof(T));
    DeleteByQueryOperation operationQuery = new DeleteByQueryOperation(new IndexQuery()
    {
      Query = $"from {collectionName} c {querySuffix ?? string.Empty}",
      QueryParameters = parameters
    }, new QueryOperationOptions { AllowStale = true });

    Operation operation = await Raven.Operations.SendAsync(operationQuery);

    await operation.WaitForCompletionAsync();
  }


  /// <inheritdoc />
  public void Purge<T>(string querySuffix = null, Parameters parameters = null)
  {
    string collectionName = Raven.Conventions.FindCollectionName(typeof(T));
    DeleteByQueryOperation operationQuery = new DeleteByQueryOperation(new IndexQuery()
    {
      Query = $"from {collectionName} c {querySuffix ?? string.Empty}",
      QueryParameters = parameters
    }, new QueryOperationOptions { AllowStale = true });

    Raven.Operations.Send(operationQuery);
  }
}


public enum ZeroSessionResolution
{
  Reuse = 0,
  Create = 1
}


public interface IZeroStore
{
  /// <summary>
  /// Get underlying raven document store
  /// </summary>
  IDocumentStore Raven { get; }

  /// <summary>
  /// Use a specific session
  /// </summary>
  IZeroDocumentSession Session(string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null);

  /// <summary>
  /// Purges a collection
  /// </summary>
  Task PurgeAsync<T>(string querySuffix = null, Parameters parameters = null);

  /// <summary>
  /// Purges a collection
  /// </summary>
  void Purge<T>(string querySuffix = null, Parameters parameters = null);
}