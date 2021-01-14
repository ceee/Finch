using Raven.Client.Documents.Session;
using System;
using zero.Core.Database;

namespace zero.Core.Collections
{
  public abstract class CollectionSession : ICollectionSession
  {
    private IAsyncDocumentSession _session;
    private string _database;

    public CollectionSession(IZeroContext context)
    {
      Context = context;
      Store = context.Store;
      Database = Store.ResolvedDatabase;
    }


    /// <summary>
    /// Zero context
    /// </summary>
    protected readonly IZeroContext Context;

    /// <summary>
    /// Document store
    /// </summary>
    protected readonly IZeroStore Store;

    /// <summary>
    /// Create an an async document session
    /// </summary>
    protected IAsyncDocumentSession Session
    {
      get
      {
        if (_session != null)
        {
          return _session;
        }
        _session = Store.OpenAsyncSession(Database ?? Store.ResolvedDatabase);
        _session.Advanced.WaitForIndexesAfterSaveChanges(throwOnTimeout: false);
        return _session;
      }
    }

    /// <inheritdoc />
    public string Database
    {
      get => _database;
      set
      {
        if (value != _database)
        {
          _session?.Dispose();
          _session = null;
          _database = value;
        }
      }
    }

    /// <inheritdoc />
    public Guid Guid { get; private set; } = Guid.NewGuid();


    /// <inheritdoc />
    public virtual void ApplyScope(string scope)
    {
      Database = scope is "shared" or "core" ? Context.Options.Raven.Database : Store.ResolvedDatabase;
    }


    /// <inheritdoc />
    public void Dispose()
    {
      Session?.Dispose();
    }
  }


  public interface ICollectionSession : IDisposable
  {
    /// <summary>
    /// Guid for this instance
    /// </summary>
    Guid Guid { get; }

    /// <summary>
    /// The database to operate on.
    /// Is null by default, which uses the database from the resolved application.
    /// </summary>
    string Database { get; set; }

    /// <summary>
    /// Applies the scope to the service instance
    /// </summary>
    void ApplyScope(string scope);
  }
}
