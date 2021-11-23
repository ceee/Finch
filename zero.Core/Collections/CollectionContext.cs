namespace zero.Collections;

public class CollectionContext : ICollectionContext
{
  public IZeroStore Store { get; private set; }

  public IZeroContext Context { get; private set; }

  public IZeroOptions Options { get; private set; }

  public IInterceptors Interceptors { get; private set; }

  public ICollectionOperations Operations { get; private set; }


  public CollectionContext(IZeroContext context, IInterceptors interceptors, ICollectionOperations operations)
  {
    Store = context.Store;
    Options = context.Options;
    Context = context;
    Interceptors = interceptors;
    Operations = operations;
  }
}


public interface ICollectionContext
{
  IZeroStore Store { get; }

  IZeroContext Context { get; }

  IZeroOptions Options { get; }

  IInterceptors Interceptors { get; }

  ICollectionOperations Operations { get; }
}