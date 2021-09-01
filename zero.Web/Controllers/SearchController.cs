using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Services;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  public class SearchController : BackofficeController
  {
    IBackofficeSearchService SearchService;

    public SearchController(IBackofficeSearchService searchService)
    {
      SearchService = searchService;
    }

    public async Task<ListResult<ZeroEntity>> Query([FromQuery] string query) => await SearchService.Query(query);
  }
}
