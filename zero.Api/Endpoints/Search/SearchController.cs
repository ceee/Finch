using Microsoft.AspNetCore.Mvc;
using zero.Api.Filters;

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
    return await Service.Query(query.Search);
  }
}