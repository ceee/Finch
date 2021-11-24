using Raven.Client.Documents.Linq;

namespace zero.Stores;

public static class EntityStoreExtensions
{
  /// <summary>
  /// Stream the collection
  /// </summary>
  public static IAsyncEnumerable<T> Stream<T>(this IEntityStore<T> store) where T : ZeroIdEntity, new() => store.Stream(null);


  /// <summary>
  /// Deletes an entity by Id
  /// </summary>
  public static async Task<EntityResult<T>> Delete<T>(this IEntityStore<T> store, string id) where T : ZeroIdEntity, new() => await store.Delete(await store.Load(id));


  /// <summary>
  /// Deletes entities by Id
  /// </summary>
  public static async Task<int> Delete<T>(this IEntityStore<T> store, IEnumerable<string> ids) where T : ZeroIdEntity, new() => await store.Delete((await store.Load(ids)).Select(x => x.Value));


  /// <summary>
  /// Deletes entities
  /// </summary>
  public static async Task<int> Delete<T>(this IEntityStore<T> store, IEnumerable<T> models) where T : ZeroIdEntity, new()
  {
    int successCount = 0;

    foreach (T model in models)
    {
      EntityResult<T> result = await store.Delete(model);
      successCount += result.IsSuccess ? 1 : 0;
    }

    return successCount;
  }


  /// <summary>
  /// Deletes an entity by Id with all descendents
  /// </summary>
  public static async Task<EntityResult<string[]>> DeleteWithDescendants<T>(this ITreeEntityStore<T> store, string id) where T : ZeroIdEntity, IZeroTreeEntity, new() => await store.DeleteWithDescendants(await store.Load(id));
}