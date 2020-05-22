using Raven.Client.Documents;

namespace zero.Core.Api
{
  public class BackofficeStore : IBackofficeStore
  {
    public IDocumentStore Raven { get; private set; }

    public IApplicationContext AppContext { get; private set; }


    public BackofficeStore(IDocumentStore raven, IApplicationContext appContext)
    {
      Raven = raven;
      AppContext = appContext;
    }
  }


  public interface IBackofficeStore
  {
    IDocumentStore Raven { get; }

    IApplicationContext AppContext { get; }
  }
}