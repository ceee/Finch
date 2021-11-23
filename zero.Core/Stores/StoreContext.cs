namespace zero.Stores;

public class StoreContext : IStoreContext
{
  public IZeroStore Store { get; private set; }

  public IZeroContext Context { get; private set; }

  public IZeroOptions Options { get; private set; }

  public IInterceptors Interceptors { get; private set; }

  public IStoreOperations Operations { get; private set; }


  public StoreContext(IZeroContext context, IInterceptors interceptors, IStoreOperations operations)
  {
    Store = context.Store;
    Options = context.Options;
    Context = context;
    Interceptors = interceptors;
    Operations = operations;
  }
}


public interface IStoreContext
{
  IZeroStore Store { get; }

  IZeroContext Context { get; }

  IZeroOptions Options { get; }

  IInterceptors Interceptors { get; }

  IStoreOperations Operations { get; }
}