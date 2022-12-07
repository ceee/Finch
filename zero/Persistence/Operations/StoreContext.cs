namespace zero.Persistence;

public class StoreContext
{
  public IZeroStore Store { get; private set; }

  public IZeroContext Context { get; private set; }

  public IZeroOptions Options { get; private set; }

  public IInterceptors Interceptors { get; private set; }

  public IServiceProvider Services { get; private set; }

  public IMessageAggregator Messages { get; private set; }


  public StoreContext(IZeroContext context, IInterceptors interceptors, IServiceProvider serviceProvider, IMessageAggregator messages)
  {
    Store = context.Store;
    Options = context.Options;
    Context = context;
    Interceptors = interceptors;
    Services = serviceProvider;
    Messages = messages;
  }
}