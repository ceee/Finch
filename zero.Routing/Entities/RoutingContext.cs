using zero.Core;

namespace zero.Routing;

public class RoutingContext : IRoutingContext
{
  public IZeroStore Store { get; private set; }

  public IZeroContext Context { get; private set; }

  public IZeroDocumentSession Session { get; private set; }


  public RoutingContext(IZeroStore store, IZeroContext context, IZeroDocumentSession session)
  {
    Store = store;
    Context = context;
    Session = session;
  }
}


public interface IRoutingContext
{
  IZeroStore Store { get; }

  IZeroContext Context { get; }

  IZeroDocumentSession Session { get; }
}