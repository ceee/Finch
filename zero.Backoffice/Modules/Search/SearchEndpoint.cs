using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Modules;

[ZeroAuthorize]
public class SearchController : BackofficeController
{
  IBackofficeSearchService SearchService;

  public SearchController(IBackofficeSearchService searchService)
  {
    SearchService = searchService;
  }

  public async Task<ListResult<SearchResult>> Query([FromQuery] string query) => await SearchService.Query(query);
}