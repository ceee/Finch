using Raven.Client.Documents;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class BackofficeStore : IBackofficeStore
  {
    public IDocumentStore Raven { get; private set; }

    public IApplicationContext AppContext { get; private set; }

    public IZeroOptions Options { get; private set; }

    public IAuthenticationApi Auth { get; private set; }


    public BackofficeStore(IDocumentStore raven, IApplicationContext appContext, IZeroOptions options, IAuthenticationApi authenticationApi)
    {
      Raven = raven;
      AppContext = appContext;
      Options = options;
      Auth = authenticationApi;
    }
  }


  public interface IBackofficeStore
  {
    IDocumentStore Raven { get; }

    IApplicationContext AppContext { get; }

    IZeroOptions Options { get; }

    IAuthenticationApi Auth { get; }
  }
}