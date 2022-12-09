using Raven.Client.Documents.Session;

namespace zero.Raven;

public class ZeroStore : IZeroStore
{
  public ZeroStore(IZeroDocumentStore raven, IZeroOptions options) : base()
  {
    Options = options;
    Raven = raven;
    //Database = null;
  }

  protected IZeroOptions Options { get; set; }
  protected Dictionary<string, IZeroDocumentSession> ScopedSessions { get; set; } = new();
  private const string NullDb = "__default__";


  /// <inheritdoc />
  public IZeroDocumentStore Raven { get; private set; }


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
  IZeroDocumentStore Raven { get; }

  /// <summary>
  /// Use a specific session
  /// </summary>
  IZeroDocumentSession Session(string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null);
}