using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Session;
using System.Threading;

namespace zero.Core.Database
{
  public interface IZeroStore
  {
    IDocumentStore RavenStore { get; }

    BulkInsertOperation BulkInsert(CancellationToken token = default);
    BulkInsertOperation BulkInsert(string database, CancellationToken token = default);
    IAsyncDocumentSession OpenAsyncSession();
    IAsyncDocumentSession OpenAsyncSession(SessionOptions options);
    IAsyncDocumentSession OpenAsyncSession(string database);
    IDocumentSession OpenSession();
    IDocumentSession OpenSession(SessionOptions options);
    IDocumentSession OpenSession(string database);
  }

  public class ZeroStore : DocumentStore, IZeroStore
  {
    public ZeroStore() : base()
    {
      CurrentDatabase = "laola.hofbauer";
    }

    /// <summary>
    /// Get underlying raven document store
    /// </summary>
    public IDocumentStore RavenStore => this;

    /// <summary>
    /// Database for the currently resolved application
    /// </summary>
    public string CurrentDatabase { get; private set; }


    /// <inheritdoc />
    public override IAsyncDocumentSession OpenAsyncSession(string database)
    {
      return base.OpenAsyncSession(database);
    }

    /// <inheritdoc />
    public override IAsyncDocumentSession OpenAsyncSession(SessionOptions options)
    {
      options.Database = options.Database ?? CurrentDatabase;
      return base.OpenAsyncSession(options);
    }

    /// <inheritdoc />
    public override IAsyncDocumentSession OpenAsyncSession()
    {
      return base.OpenAsyncSession(CurrentDatabase);
    }

    /// <inheritdoc />
    public override IDocumentSession OpenSession(SessionOptions options)
    {
      options.Database = options.Database ?? CurrentDatabase;
      return base.OpenSession(options);
    }

    /// <inheritdoc />
    public override IDocumentSession OpenSession(string database)
    {
      return base.OpenSession(database);
    }

    /// <inheritdoc />
    public override IDocumentSession OpenSession()
    {
      return base.OpenSession(CurrentDatabase);
    }

    /// <inheritdoc />
    public override BulkInsertOperation BulkInsert(string database, CancellationToken token = default)
    {
      return base.BulkInsert(database, token);
    }

    /// <summary>
    /// Create a bulk insert operation for the resolved database
    /// </summary>
    public BulkInsertOperation BulkInsert(CancellationToken token = default)
    {
      return base.BulkInsert(CurrentDatabase, token);
    }
  }
}
