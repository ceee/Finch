//using Microsoft.AspNetCore.Mvc;

//namespace zero.Api.Modules;

//[ZeroAuthorize]
//public class SearchController : BackofficeController
//{
//  IBackofficeSearchService SearchService;

//  public SearchController(IBackofficeSearchService searchService)
//  {
//    SearchService = searchService;
//  }

//  public async Task<Paged<SearchResult>> Query([FromQuery] string query) => await SearchService.Query(query);
//}