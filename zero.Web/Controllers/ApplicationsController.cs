using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Read)]
  public class ApplicationsController : BackofficeController
  {
    private IApplicationsApi Api { get; set; }

    private ZeroOptions Options { get; set; }


    public ApplicationsController(IZeroConfiguration config, IApplicationsApi api, IMapper mapper, IToken token, IOptionsMonitor<ZeroOptions> options) : base(config, mapper, token)
    {
      Api = api;
      Options = options.CurrentValue;
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public IActionResult GetEmpty()
    {
      return Json(new ApplicationEditModel());
    }


    /// <summary>
    /// Get translation by id
    /// </summary>  
    public async Task<IActionResult> GetById([FromQuery] string id)
    {
      return await As<Application, ApplicationEditModel>(await Api.GetById(id));
    }


    /// <summary>
    /// Get all translations
    /// </summary>    
    public async Task<IActionResult> GetAll([FromQuery] ListQuery<Application> query = null)
    {
      if (query == null)
      {
        return await As<Application, ApplicationListModel>(await Api.GetAll());
      }

      return await As<Application, ApplicationListModel>(await Api.GetByQuery(query));
    }


    /// <summary>
    /// Get all available features to select
    /// </summary>
    public IActionResult GetAllFeatures()
    {
      return Json(Options.Features);
    }


    /// <summary>
    /// Save translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Write)]
    public async Task<IActionResult> Save([FromBody] ApplicationEditModel model)
    {
      Application entity = await Mapper.Map(model, await Api.GetById(model.Id));
      return await As<Application, ApplicationEditModel>(await Api.Save(entity));
    }


    /// <summary>
    /// Deletes a translation
    /// </summary>
    [ZeroAuthorize(Permissions.Settings.Applications, PermissionsValue.Write)]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return await As<Application, ApplicationEditModel>(await Api.Delete(id));
    }
  }
}
