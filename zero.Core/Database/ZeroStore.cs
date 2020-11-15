using Raven.Client.Documents;
using Raven.Client.Documents.BulkInsert;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Database
{
  public class ZeroStore : DocumentStore, IZeroStore
  {
    protected IZeroOptions Options { get; set; }

    public ZeroStore(IZeroOptions options) : base()
    {
      Options = options;
      Database = null;
    }


    internal async Task SetContext()
    {

    }


    /// <inheritdoc />
    public IDocumentStore Raven => this;


    /// <inheritdoc />
    public OperationExecutor GetOperationExecutor(string database = null)
    {
      if (database.IsNullOrEmpty())
      {
        return Operations;
      }
      else
      {
        return Operations.ForDatabase(database);
      }
    }
  }


  public interface IZeroStore : IDocumentStore
  {
    /// <summary>
    /// Get underlying raven document store
    /// </summary>
    IDocumentStore Raven { get; }

    /// <summary>
    /// Get operation executor
    /// </summary>
    public OperationExecutor GetOperationExecutor(string database = null);
  }
}
