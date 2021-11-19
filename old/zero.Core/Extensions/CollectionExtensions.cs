using zero.Core.Collections;

namespace zero.Core.Extensions
{
  public static class CollectionExtensions
  {
    /// <summary>
    /// Shared scope for the current collection instance
    /// </summary>
    public static CollectionScope SharedScope<T>(this T collection) where T : ICollectionBase
    {
      return new CollectionScope(collection, "shared");
    }
  }
}
