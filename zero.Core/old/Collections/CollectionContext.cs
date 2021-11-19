namespace zero.Core.Collections
{
  public class CollectionContext<T> : CollectionContext, ICollectionContext<T> where T : ZeroIdEntity, new()
  {
    public IInterceptorRunner<T> Interceptors { get; private set; }

    public CollectionContext(IZeroContext context, IInterceptorRunner<T> interceptors) : base(context)
    {
      Interceptors = interceptors;
    }
  }


  public class CollectionContext : ICollectionContext
  {
    public IZeroStore Store { get; private set; }

    public IZeroContext Context { get; private set; }

    public IZeroOptions Options { get; private set; }


    public CollectionContext(IZeroContext context)
    {
      Store = context.Store;
      Options = context.Options;
      Context = context;
    }
  }


  public interface ICollectionContext<T> : ICollectionContext where T : ZeroIdEntity, new()
  {
    IInterceptorRunner<T> Interceptors { get; }
  }


  public interface ICollectionContext
  {
    IZeroStore Store { get; }

    IZeroContext Context { get; }

    IZeroOptions Options { get; }
  }
}