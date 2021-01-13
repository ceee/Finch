using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Integrations;

namespace zero.Web.Controllers
{
  //[ZeroAuthorize(Permissions.Sections.Spaces, PermissionsValue.True)]
  public class IntegrationsController : BackofficeController
  {
    IIntegrationsCollection Integrations;
    IIntegrationService Types;

    public IntegrationsController(IIntegrationsCollection integrations, IIntegrationService types)
    {
      Integrations = integrations;
      Types = types;
    }


    public async Task<IActionResult> GetAll()
    {
      return Ok(new
      {
        //available = await Types.GetAvailable(),
        //activated = await Types.GetActivated()
      });
    }


    public IActionResult GetEmptySettings([FromQuery] string alias)
    {
      //IIntegration integration = Types.GetByAlias(alias);
      //IIntegrationModel model = integration != null ? Activator.CreateInstance(integration.ModelType) as IIntegrationModel : null;

      //if (model != null)
      //{
      //  model.IntegrationAlias = integration.Alias;
      //}

      return Ok(Edit(default(IIntegration)));
    }


    public async Task<IActionResult> GetSettingsByAlias([FromQuery] string alias)
    {
      //IIntegrationModel content = await Integrations.GetByAlias(alias);
      return Ok(Edit(default(IIntegration)));
    }


    public async Task<IActionResult> GetSettingsById([FromQuery] string id)
    {
      //IIntegrationModel content = await Integrations.GetById(id);
      return Ok(Edit(default(IIntegration)));
    }


    public async Task<IActionResult> Save([FromBody] IIntegration model)
    {
      return Ok();
      //return Ok(await Integrations.Save(model));
    }

    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return Ok(await Integrations.DeleteById(id));
    }
  }
}
