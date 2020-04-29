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
    /// Get all list collections
    /// </summary>
    public IActionResult GetCollections([FromQuery] string id)
    {
      return Json(Api.GetCollections());
    }


    /// <summary>
    /// Get list items in a collection
    /// </summary>    
    //public async Task<IActionResult> GetAll([FromQuery] string alias, [FromQuery] ListQuery<Country> query = null)
    //{
    //  return await As<Country, CountryListModel>(await Api.GetAll(alias));
    //}
  }
}
