using Raven.Client;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using System;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Options;

namespace zero.Core.Extensions
{
  internal static class ZeroStoreExtensions
  {
    /// <summary>
    /// Opens a session for the core database
    /// </summary>
    public static IAsyncDocumentSession OpenCoreSession(this IZeroStore store, IZeroOptions options)
    {
      return store.OpenAsyncSession(options.Raven.Database);
    }


    /// <summary>
    /// Purges a collection
    /// </summary>
    public static async Task PurgeAsync<T>(this IZeroStore store, string database = null, string querySuffix = null, Parameters parameters = null)
    {
      var collectionName = store.Conventions.FindCollectionName(typeof(T));

      var operationQuery = new DeleteByQueryOperation(new IndexQuery()
      {
        Query = $"from {collectionName} c {querySuffix ?? String.Empty}",
        QueryParameters = parameters
      }, new QueryOperationOptions
      {
        AllowStale = true
      });

      Operation operation = await store.GetOperationExecutor(database).SendAsync(operationQuery);

      await operation.WaitForCompletionAsync();
    }
  }
}
