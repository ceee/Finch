using zero.Core.Collections;
using zero.Core.Database;
using zero.Core.Options;

namespace zero.Core.Collections
{
  public class CollectionContext : ICollectionContext
  {
    public IZeroStore Store { get; private set; }

    public IZeroContext Context { get; private set; }

    public IZeroOptions Options { get; private set; }


    public CollectionContext(IZeroStore store, IZeroContext context, IZeroOptions options)
    {
      Store = store;
      Context = context;
      Options = options;
    }
  }


  public interface ICollectionContext
  {
    IZeroStore Store { get; }

    IZeroContext Context { get; }

    IZeroOptions Options { get; }
  }
}