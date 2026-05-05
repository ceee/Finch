namespace Mixtape.Raven;

public class StoreContext
{
  public IMixtapeStore Store { get; private set; }

  public IMixtapeContext Context { get; private set; }

  public IMixtapeOptions Options { get; private set; }

  public IInterceptors Interceptors { get; private set; }

  public IServiceProvider Services { get; private set; }

  public IMessageAggregator Messages { get; private set; }


  public StoreContext(IMixtapeContext context, IMixtapeStore store, IInterceptors interceptors, IServiceProvider serviceProvider, IMessageAggregator messages)
  {
    Store = store;
    Options = context.Options;
    Context = context;
    Interceptors = interceptors;
    Services = serviceProvider;
    Messages = messages;
  }
}