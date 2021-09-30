using System;

namespace zero.Core.Collections
{
  public sealed class CollectionScope : IDisposable
  {
    ICollectionBase _collection;


    internal CollectionScope(ICollectionBase collection, string scope)
    {
      _collection = collection;
      _collection.ApplyScope(scope);
    }

    public void Dispose()
    {
      _collection?.ApplyScope(null);
    }
  }
}
