using zero.Core.Database;
using zero.Core.Messages;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class BackofficeStore : IBackofficeStore
  {
    public IZeroStore Store { get; private set; }

    public IZeroContext Context { get; private set; }

    public IZeroOptions Options { get; private set; }

    public IAuthenticationService Auth { get; private set; }

    public IMessageAggregator Messages { get; private set; }


    public BackofficeStore(IZeroStore store, IZeroContext context, IZeroOptions options, IAuthenticationService authenticationApi, IMessageAggregator messages)
    {
      Store = store;
      Context = context;
      Options = options;
      Auth = authenticationApi;
      Messages = messages;
    }
  }


  public interface IBackofficeStore
  {
    IZeroStore Store { get; }

    IZeroContext Context { get; }

    IZeroOptions Options { get; }

    IAuthenticationService Auth { get; }

    IMessageAggregator Messages { get; }
  }
}