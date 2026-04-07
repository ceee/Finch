namespace Finch.Raven;

public class StoreContext
{
  public IFinchStore Store { get; private set; }

  public IFinchContext Context { get; private set; }

  public IFinchOptions Options { get; private set; }

  public IInterceptors Interceptors { get; private set; }

  public IServiceProvider Services { get; private set; }

  public IMessageAggregator Messages { get; private set; }


  public StoreContext(IFinchContext context, IFinchStore store, IInterceptors interceptors, IServiceProvider serviceProvider, IMessageAggregator messages)
  {
    Store = store;
    Options = context.Options;
    Context = context;
    Interceptors = interceptors;
    Services = serviceProvider;
    Messages = messages;
  }
}