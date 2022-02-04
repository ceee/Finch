using Microsoft.AspNetCore.Mvc;
using zero.Search;

namespace zero.Api.Endpoints.Search;

public class SearchController : ZeroApiController
{
  protected ISearchService Service { get; set; }

  public SearchController(ISearchService service)
  {
    Service = service;
  }


  [HttpGet("")]
  public async Task<ActionResult<Paged>> Query([FromQuery] ListQuery<SearchResult> query)
  {
    if (!query.Search.HasValue())
    {
      return new Paged<SearchResult>(new List<SearchResult>(), 0, query.Page, query.PageSize);
    }
    return await Service.Query(query.Search, query.Page, query.PageSize);
  }
}