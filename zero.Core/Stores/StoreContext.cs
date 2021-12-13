namespace zero.Stores;

public class StoreContext : IStoreContext
{
  public IZeroStore Store { get; private set; }

  public IZeroContext Context { get; private set; }

  public IZeroOptions Options { get; private set; }

  public IInterceptors Interceptors { get; private set; }

  public IStoreCache Cache { get; private set; }


  public StoreContext(IZeroContext context, IInterceptors interceptors, IStoreCache cache)
  {
    Store = context.Store;
    Options = context.Options;
    Context = context;
    Interceptors = interceptors;
    Cache = cache;
  }
}


public interface IStoreContext
{
  IZeroStore Store { get; }

  IZeroContext Context { get; }

  IZeroOptions Options { get; }

  IInterceptors Interceptors { get; }

  IStoreCache Cache { get; }
}