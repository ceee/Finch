using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using zero.Core.Integrations;

namespace zero.Web.Controllers
{
  //[ZeroAuthorize(Permissions.Sections.Spaces, PermissionsValue.True)]
  public class IntegrationsController : BackofficeController
  {
    IIntegrationService Integrations;

    public IntegrationsController(IIntegrationService integrations)
    {
      Integrations = integrations;
    }


    public async Task<IActionResult> GetAll()
    {
      return Ok(new
      {
        available = await Integrations.GetAvailable(),
        activated = await Integrations.GetActivated()
      });
    }


    public IActionResult GetEmptySettings([FromQuery] string alias)
    {
      IIntegration integration = Integrations.GetByAlias(alias);
      IIntegrationSettings model = integration != null ? Activator.CreateInstance(integration.SettingsType) as IIntegrationSettings : null;

      if (model != null)
      {
        model.IntegrationAlias = integration.Alias;
      }

      return Ok(Edit(model));
    }


    public async Task<IActionResult> GetSettingsByAlias([FromQuery] string alias)
    {
      IIntegrationSettings content = await Integrations.GetSettingsByAlias(alias);
      return Ok(Edit(content));
    }


    public async Task<IActionResult> GetSettingsById([FromQuery] string id)
    {
      IIntegrationSettings content = await Integrations.GetSettingsById(id);
      return Ok(Edit(content));
    }


    public async Task<IActionResult> Save([FromBody] IIntegrationSettings model)
    {
      return Ok(await Integrations.Save(model));
    }

    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return Ok(await Integrations.Delete(id));
    }
  }
}
