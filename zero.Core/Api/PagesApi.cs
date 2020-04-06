using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class PagesApi : IPagesApi
  {
    protected IDocumentStore Raven { get; private set; }


    public PagesApi(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public async Task<IList<Application>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Application>().ToListAsync();
      }
    }
  }


  public interface IPagesApi
  {
    /// <summary>
    /// Get all available zero applications
    /// </summary>
    Task<IList<Application>> GetAll();
  }
}
