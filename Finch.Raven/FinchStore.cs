namespace Finch.Raven;

public class FinchStore : IFinchStore
{
  public FinchStore(IDocumentStore raven, IFinchOptions options) : base()
  {
    Options = options;
    Raven = raven;
    //Database = null;
  }

  protected IFinchOptions Options { get; set; }
  protected Dictionary<string, IAsyncDocumentSession> ScopedSessions { get; set; } = new();
  private const string NullDb = "__default__";


  /// <inheritdoc />
  public IDocumentStore Raven { get; }


  /// <inheritdoc />
  public IAsyncDocumentSession Session(FinchSessionResolution resolution = FinchSessionResolution.Reuse, SessionOptions options = null)
  {
    options ??= new SessionOptions();

    if (resolution == FinchSessionResolution.Create)
    {
      return Raven.OpenAsyncSession(options);
    }

    if (!ScopedSessions.TryGetValue(Raven.Database, out IAsyncDocumentSession session))
    {
      session = Raven.OpenAsyncSession(options);
      ScopedSessions.TryAdd(Raven.Database, session);
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


public enum FinchSessionResolution
{
  Reuse = 0,
  Create = 1
}


public interface IFinchStore
{
  /// <summary>
  /// Get underlying raven document store
  /// </summary>
  IDocumentStore Raven { get; }

  /// <summary>
  /// Use a specific session
  /// </summary>
  IAsyncDocumentSession Session(FinchSessionResolution resolution = FinchSessionResolution.Reuse, SessionOptions options = null);

  /// <summary>
  /// Purges a collection
  /// </summary>
  Task PurgeAsync<T>(string querySuffix = null, Parameters parameters = null);

  /// <summary>
  /// Purges a collection
  /// </summary>
  void Purge<T>(string querySuffix = null, Parameters parameters = null);
}