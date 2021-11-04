using zero.Core.Database;
using zero.Core.Options;

namespace zero.Core.Routing
{
  public class RoutingContext : IRoutingContext
  {
    public IZeroStore Store { get; private set; }

    public IZeroContext Context { get; private set; }

    public IZeroOptions Options { get; private set; }

    public IZeroDocumentSession Session { get; private set; }


    public RoutingContext(IZeroStore store, IZeroContext context, IZeroOptions options, IZeroDocumentSession session)
    {
      Store = store;
      Context = context;
      Options = options;
      Session = session;
    }
  }


  public interface IRoutingContext
  {
    IZeroStore Store { get; }

    IZeroContext Context { get; }

    IZeroOptions Options { get; }

    IZeroDocumentSession Session { get; }
  }
}
