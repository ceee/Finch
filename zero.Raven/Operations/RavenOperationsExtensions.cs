using Raven.Client.Documents.Linq;

namespace zero.Raven;

public static class RavenOperationsExtensions
{
  /// <summary>
  /// Stream the collection
  /// </summary>
  public static IAsyncEnumerable<T> Stream<T>(this IRavenOperations ops) where T : ZeroIdEntity, new() => ops.Stream<T>(null);


  /// <summary>
  /// Deletes an entity by Id
  /// </summary>
  public static async Task<Result<T>> Delete<T>(this IRavenOperations ops, string id) where T : ZeroIdEntity, new() => await ops.Delete(await ops.Load<T>(id));


  /// <summary>
  /// Deletes entities by Id
  /// </summary>
  public static async Task<int> Delete<T>(this IRavenOperations ops, IEnumerable<string> ids) where T : ZeroIdEntity, new() => await ops.Delete((await ops.Load<T>(ids)).Select(x => x.Value));


  /// <summary>
  /// Deletes entities
  /// </summary>
  public static async Task<int> Delete<T>(this IRavenOperations ops, IEnumerable<T> models) where T : ZeroIdEntity, new()
  {
    int successCount = 0;

    foreach (T model in models)
    {
      Result<T> result = await ops.Delete(model);
      successCount += result.IsSuccess ? 1 : 0;
    }

    return successCount;
  }

  /// <summary>
  /// Deletes an entity by Id with all descendents
  /// </summary>
  public static async Task<Result<string[]>> DeleteWithDescendants<T>(this IRavenOperations ops, string id) where T : ZeroIdEntity, ISupportsTrees, new() => await ops.DeleteWithDescendants(await ops.Load<T>(id));
}