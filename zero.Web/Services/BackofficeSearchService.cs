using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Web.Services
{
  public class BackofficeSearchService : IBackofficeSearchService
  {
    protected IZeroDocumentSession Session { get; private set; }

    public BackofficeSearchService(IZeroDocumentSession session)
    {
      Session = session;
    }


    public async Task<ListResult<ZeroEntity>> Query(string searchTerm)
    {
      List<ZeroEntity> results = await Session.Query<Backoffice_Search.Result, Backoffice_Search>()
        .Statistics(out QueryStatistics stats)
        .Search(x => x.Name, searchTerm, 2)
        .Search(x => x.Fields, searchTerm, 1, SearchOptions.Or)
        .Paging(1, 20)
        .ProjectInto<ZeroEntity>()
        .ToListAsync();

      return new ListResult<ZeroEntity>(results, stats.TotalResults, 1, 20);
    }
  }

  public interface IBackofficeSearchService
  {
    Task<ListResult<ZeroEntity>> Query(string searchTerm);
  }
}
