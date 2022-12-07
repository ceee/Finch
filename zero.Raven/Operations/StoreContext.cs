namespace zero.Raven;

public class StoreContext
{
  public IZeroStore Store { get; private set; }

  public IZeroContext Context { get; private set; }

  public IZeroOptions Options { get; private set; }

  public IInterceptors Interceptors { get; private set; }

  public IServiceProvider Services { get; private set; }

  public IMessageAggregator Messages { get; private set; }


  public StoreContext(IZeroContext context, IZeroStore store, IInterceptors interceptors, IServiceProvider serviceProvider, IMessageAggregator messages)
  {
    Store = store;
    Options = context.Options;
    Context = context;
    Interceptors = interceptors;
    Services = serviceProvider;
    Messages = messages;
  }
}