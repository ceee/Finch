using Raven.Client.Documents;
using Raven.Client.Documents.Operations.CompareExchange;
using System;
using System.Threading.Tasks;
using zero.Core.Utils;

namespace zero.Core.Extensions
{
  public static class DocumentStoreExtensions
  {
    /// <summary>
    /// Create a new unique Id
    /// </summary>
    public static string Id(this IDocumentStore store, int length = -1)
    {
      return IdGenerator.Create(length);
    }


    /// <summary>
    /// Reserves a key cluster-wide
    /// </summary>
    public static async Task<bool> ReserveAsync(this IDocumentStore store, string key, string value = null)
    {
      if (String.IsNullOrWhiteSpace(key))
      {
        return false;
      }
      if (value == null)
      {
        value = key;
      }
      var operation = new PutCompareExchangeValueOperation<string>(key, value, 0);
      CompareExchangeResult<string> result = await store.Operations.SendAsync(operation).ConfigureAwait(false);
      return result.Successful;
    }


    /// <summary>
    /// Removes a cluster-wide key reservation
    /// </summary>
    public static async Task<bool> RemoveReservationAsync(this IDocumentStore store, string key)
    {
      if (!String.IsNullOrWhiteSpace(key))
      {
        return false;
      }

      CompareExchangeValue<string> readResult = store.Operations.Send(new GetCompareExchangeValueOperation<string>(key));

      if (readResult == null)
      {
        return false;
      }

      DeleteCompareExchangeValueOperation<string> operation = new DeleteCompareExchangeValueOperation<string>(key, readResult.Index);

      CompareExchangeResult<string> result = await store.Operations.SendAsync(operation).ConfigureAwait(false);
      return result.Successful;
    }
  }
}
