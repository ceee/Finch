using Raven.Client.Documents;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Models;

namespace zero.Web.Services
{
  public class BackofficeSearchService : IBackofficeSearchService
  {
    protected IZeroDocumentSession Session { get; private set; }

    public BackofficeSearchService(IZeroDocumentSession session)
    {
      Session = session;
    }


    public async Task<ListResult<SearchResult>> Query(string searchTerm)
    {
      string[] searchParts = searchTerm.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(x =>
      {
        return "*" + x + "*";
      }).ToArray();

      List<ZeroEntity> results = await Session.Query<SearchIndexResult, Backoffice_Search>()
        .Statistics(out QueryStatistics stats)
        .Search(x => x.Name, searchParts, 2, @operator: SearchOperator.And)
        .Search(x => x.Fields, searchParts, 1, SearchOptions.Or, @operator: SearchOperator.And)
        .Paging(1, 20)
        .ProjectInto<ZeroEntity>()
        .ToListAsync();

      List<SearchResult> items = new();

      foreach (ZeroEntity result in results)
      {
        items.Add(new()
        {
          Id = result.Id,
          Name = result.Name,
          IsActive = result.IsActive,
          Url = "/" 
        });
      }

      return new ListResult<SearchResult>(items, stats.TotalResults, 1, 20);
    }
  }

  public interface IBackofficeSearchService
  {
    Task<ListResult<SearchResult>> Query(string searchTerm);
  }
}
