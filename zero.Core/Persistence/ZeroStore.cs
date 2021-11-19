using Raven.Client.Documents.Session;
using System.Collections.Generic;
using zero.Core.Options;

namespace zero;

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

  /// <inheritdoc />
  public string ResolvedDatabase { get; set; }


  /// <inheritdoc />
  public IZeroDocumentStore Raven { get; private set; }


  /// <inheritdoc />
  public IZeroDocumentSession Session(string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null)
  {
    database ??= ResolvedDatabase;
    options ??= new SessionOptions() { Database = database };

    if (resolution == ZeroSessionResolution.Create)
    {
      return Raven.OpenAsyncSession(options) as IZeroDocumentSession;
    }

    if (!ScopedSessions.TryGetValue(database, out IZeroDocumentSession session))
    {
      session = Raven.OpenAsyncSession(options) as IZeroDocumentSession;
      ScopedSessions.TryAdd(database, session);
    }
      
    if (session.IsDisposed)
    {
      session = Raven.OpenAsyncSession(options) as IZeroDocumentSession;
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
}


public enum ZeroSessionResolution
{
  Reuse = 0,
  Create = 1
}


public interface IZeroStore
{
  /// <summary>
  /// The database which has been resolved from the current application
  /// </summary>
  string ResolvedDatabase { get; set; }

  /// <summary>
  /// Get underlying raven document store
  /// </summary>
  IZeroDocumentStore Raven { get; }

  /// <summary>
  /// Use a specific session
  /// </summary>
  IZeroDocumentSession Session(string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null);

  /// <summary>
  /// Use a session for the core database
  /// </summary>
  IZeroDocumentSession Session(bool global, string database = null, ZeroSessionResolution resolution = ZeroSessionResolution.Reuse, SessionOptions options = null);
}