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
  [ZeroAuthorize(Permissions.Sections.Spaces, PermissionsValue.True)]
  public class SpacesController : BackofficeController
  {
    private ISpacesApi Api { get; set; }

    public SpacesController(IZeroConfiguration config, ISpacesApi api, IMapper mapper, IToken token) : base(config, mapper, token)
    {
      Api = api;
    }


    /// <summary>
    /// Get space by alias
    /// </summary>
    public IActionResult GetByAlias([FromQuery] string alias)
    {
      return Json(Api.GetByAlias(alias));
    }


    /// <summary>
    /// Get all spaces
    /// </summary>
    public IActionResult GetAll()
    {
      return Json(Api.GetAll());
    }


    /// <summary>
    /// Get list items in a space
    /// </summary>    
    public async Task<IActionResult> GetList([FromQuery] string alias, [FromQuery] ListQuery<SpaceContent> query = null)
    {
      return Json(await Api.GetListByQuery<SpaceContent>(alias, query));
    }
  }
}
