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

    public ICollectionInterceptorHandler InterceptorHandler { get; private set; }


    public CollectionContext(IZeroStore store, IZeroContext context, IZeroOptions options, ICollectionInterceptorHandler interceptorHandler)
    {
      Store = store;
      Context = context;
      Options = options;
      InterceptorHandler = interceptorHandler;
    }
  }


  public interface ICollectionContext
  {
    IZeroStore Store { get; }

    IZeroContext Context { get; }

    IZeroOptions Options { get; }

    ICollectionInterceptorHandler InterceptorHandler { get; }
  }
}