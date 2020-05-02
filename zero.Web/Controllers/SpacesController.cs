using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.TestData;
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
      return Json(await Api.GetListByQuery(alias, query));
    }


    /// <summary>
    /// Get list items in a space
    /// </summary>    
    public async Task<IActionResult> GetContent([FromQuery] string alias, [FromQuery] string contentId = null)
    {
      TeamMember model = new TeamMember();

      if (!contentId.IsNullOrEmpty())
      {
        model = await Api.GetItem<TeamMember>(alias, contentId);
      }

      return Json(new SpaceContentEditModel()
      {
        Alias = alias,
        Model = model,
        Config = Api.GetEditorConfig(alias)
      });
    }


    /// <summary>
    /// Save content item
    /// </summary>
    public async Task<IActionResult> Save([FromBody] TeamMember model)
    {
      TeamMember member = await Mapper.Map(model, await Api.GetItem<TeamMember>(model.Id));
      return Json(await Api.Save(model.SpaceAlias, model));
    }


    /// <summary>
    /// Deletes a country
    /// </summary>
    //public async Task<IActionResult> Delete([FromQuery] string id)
    //{
    //  return await As<UserRole, UserRoleEditModel>(await Api.Delete(id));
    //}
  }
}
