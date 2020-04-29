using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Lists, PermissionsValue.True)]
  public class ListsController : BackofficeController
  {
    private IListsApi Api { get; set; }

    public ListsController(IZeroConfiguration config, IListsApi api, IMapper mapper, IToken token) : base(config, mapper, token)
    {
      Api = api;
    }


    /// <summary>
    /// Get list collection by alias
    /// </summary>
    public IActionResult GetCollectionByAlias([FromQuery] string alias)
    {
      return Json(Api.GetCollectionByAlias(alias));
    }


    /// <summary>
    /// Get all list collections
    /// </summary>
    public IActionResult GetCollections()
    {
      return Json(Api.GetCollections());
    }


    /// <summary>
    /// Get list items in a collection
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] string alias, [FromQuery] ListQuery<ListItem> query = null)
    {
      return Json(await Api.GetByQuery<ListItem>(alias, query)); 
    }
  }
}
