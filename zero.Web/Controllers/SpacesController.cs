using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Spaces, PermissionsValue.True)]
  public class SpacesController : BackofficeController
  {
    ISpacesApi Api;
    IAuthenticationApi AuthenticationApi;

    public SpacesController(ISpacesApi api, IAuthenticationApi authenticationApi)
    {
      Api = api;
      AuthenticationApi = authenticationApi;
    }


    /// <summary>
    /// Get space by alias
    /// </summary>
    public IActionResult GetByAlias([FromQuery] string alias)
    {
      if (!CanReadSpace(alias))
      {
        return new StatusCodeResult(403);
      }

      return Json(Api.GetByAlias(alias));
    }


    /// <summary>
    /// Get all spaces
    /// </summary>
    public IActionResult GetAll()
    {
      IList<Space> spaces = Api.GetAll().Where(space => CanReadSpace(space.Alias)).ToList();
      return Json(spaces);
    }


    /// <summary>
    /// Get list items in a space
    /// </summary>    
    public async Task<IActionResult> GetList([FromQuery] string alias, [FromQuery] ListQuery<SpaceContent> query = null)
    {
      if (!CanReadSpace(alias))
      {
        return new StatusCodeResult(403);
      }

      return Json(await Api.GetListByQuery(alias, query));
    }


    /// <summary>
    /// Get list items in a space
    /// </summary>    
    public IActionResult GetContent([FromQuery] string alias, [FromQuery] string contentId = null)
    {
      if (!CanReadSpace(alias))
      {
        return new StatusCodeResult(403);
      }

      //TeamMember model = new TeamMember();

      //if (!contentId.IsNullOrEmpty())
      //{
      //  model = await Api.GetItem<TeamMember>(alias, contentId);
      //}

      JsonSerializerSettings settings = JsonConvert.DefaultSettings();
      settings.TypeNameHandling = TypeNameHandling.Objects;

      return Json(new SpaceContentEditModel()
      {
        //Id = model.Id,
        Alias = alias,
        //Model = model,
        //Config = Api.GetEditorConfig(alias)
      }, settings);
    }


    /// <summary>
    /// Save content item
    /// </summary>
    public async Task<IActionResult> Save([FromBody] SpaceContentEditModel model)
    {
      if (!CanWriteSpace(model.Alias))
      {
        return new StatusCodeResult(403);
      }

      return Json(await Api.Save(model.Alias, model.Model));
    }


    /// <summary>
    /// Deletes a content item
    /// </summary>
    public async Task<IActionResult> Delete([FromQuery] string alias, [FromQuery] string id)
    {
      if (!CanWriteSpace(alias))
      {
        return new StatusCodeResult(403);
      }

      return Json(await Api.Delete(alias, id));
    }


    /// <summary>
    /// Whether the current user can read a space
    /// </summary>
    bool CanReadSpace(string alias)
    {
      Permission permission = AuthenticationApi.GetPermission(Permissions.Spaces.PREFIX + alias);
      return permission != null && permission.CanRead;
    }


    /// <summary>
    /// Whether the current user can read a space
    /// </summary>
    bool CanWriteSpace(string alias)
    {
      Permission permission = AuthenticationApi.GetPermission(Permissions.Spaces.PREFIX + alias);
      return permission != null && permission.CanWrite;
    }
  }
}
