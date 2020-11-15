using Raven.Client.Documents;
using zero.Core.Database;
using zero.Core.Messages;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class BackofficeStore : IBackofficeStore
  {
    public IZeroStore Store { get; private set; }

    public IApplicationContext AppContext { get; private set; }

    public IZeroOptions Options { get; private set; }

    public IAuthenticationApi Auth { get; private set; }

    public IMessageAggregator Messages { get; private set; }


    public BackofficeStore(IZeroStore store, IApplicationContext appContext, IZeroOptions options, IAuthenticationApi authenticationApi, IMessageAggregator messages)
    {
      Store = store;
      AppContext = appContext;
      Options = options;
      Auth = authenticationApi;
      Messages = messages;
    }
  }


  public interface IBackofficeStore
  {
    IZeroStore Store { get; }

    IApplicationContext AppContext { get; }

    IZeroOptions Options { get; }

    IAuthenticationApi Auth { get; }

    IMessageAggregator Messages { get; }
  }
}