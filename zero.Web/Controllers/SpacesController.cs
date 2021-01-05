using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Spaces, PermissionsValue.True)]
  public class SpacesController : BackofficeController
  {
    ISpacesApi Api;

    public SpacesController(ISpacesApi api)
    {
      Api = api;
    }


    public IActionResult GetByAlias([FromQuery] string alias)
    {
      if (!CanReadSpace(alias))
      {
        return new StatusCodeResult(403);
      }

      return Ok(Api.GetByAlias(alias));
    }


    public List<Space> GetAll() => Api.GetAll().Where(space => CanReadSpace(space.Alias)).ToList();


    public async Task<IActionResult> GetList([FromQuery] string alias, [FromQuery] ListBackofficeQuery<ISpaceContent> query = null)
    {
      if (!CanReadSpace(alias))
      {
        return new StatusCodeResult(403);
      }

      return Ok(await Api.GetListByQuery(alias, query));
    }


    public async Task<IActionResult> GetContent([FromQuery] string alias, [FromQuery] string contentId = null)
    {
      if (!CanReadSpace(alias))
      {
        return new StatusCodeResult(403);
      }

      Space space = Api.GetByAlias(alias);
      ISpaceContent model = null;

      if (space == null)
      {
        return new StatusCodeResult(404);
      }

      if (space.View != SpaceView.List || !contentId.IsNullOrEmpty())
      {
        model = await Api.GetItem(alias, contentId);
      }
      
      if (model == null && contentId.IsNullOrEmpty())
      {
        model = Activator.CreateInstance(space.Type) as SpaceContent;
        model.SpaceAlias = space.Alias;
      }

      return Ok(Edit(model));
    }


    /// <summary>
    /// Save content item
    /// </summary>
    public async Task<IActionResult> Save([FromBody] ISpaceContent model)
    {
      if (!CanWriteSpace(model.SpaceAlias))
      {
        return new StatusCodeResult(403);
      }

      return Ok(await Api.Save(model));
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

      return Ok(await Api.Delete(id));
    }


    /// <summary>
    /// Whether the current user can read a space
    /// </summary>
    bool CanReadSpace(string alias)
    {
      return true;
      //Permission permission = AuthenticationApi.GetPermission(Permissions.Spaces.PREFIX + alias);
      //return permission != null && permission.CanRead;
    }


    /// <summary>
    /// Whether the current user can read a space
    /// </summary>
    bool CanWriteSpace(string alias)
    {
      return true;
      //Permission permission = AuthenticationApi.GetPermission(Permissions.Spaces.PREFIX + alias);
      //return permission != null && permission.CanWrite;
    }
  }
}
