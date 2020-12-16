using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using zero.Core.Integrations;

namespace zero.Web.Controllers
{
  //[ZeroAuthorize(Permissions.Sections.Spaces, PermissionsValue.True)]
  public class IntegrationsController : BackofficeController
  {
    IIntegrationsCollection Integrations;
    IIntegrationTypeService Types;

    public IntegrationsController(IIntegrationsCollection integrations, IIntegrationTypeService types)
    {
      Integrations = integrations;
      Types = types;
    }


    public async Task<IActionResult> GetAll()
    {
      return Ok(new
      {
        available = await Types.GetAvailable(),
        activated = await Types.GetActivated()
      });
    }


    public IActionResult GetEmptySettings([FromQuery] string alias)
    {
      IIntegrationType integration = Types.GetByAlias(alias);
      IIntegration model = integration != null ? Activator.CreateInstance(integration.SettingsType) as IIntegration : null;

      if (model != null)
      {
        model.IntegrationAlias = integration.Alias;
      }

      return Ok(Edit(model));
    }


    public async Task<IActionResult> GetSettingsByAlias([FromQuery] string alias)
    {
      IIntegration content = await Integrations.GetByAlias(alias);
      return Ok(Edit(content));
    }


    public async Task<IActionResult> GetSettingsById([FromQuery] string id)
    {
      IIntegration content = await Integrations.GetById(id);
      return Ok(Edit(content));
    }


    public async Task<IActionResult> Save([FromBody] IIntegration model)
    {
      return Ok(await Integrations.Save(model));
    }

    public async Task<IActionResult> Delete([FromQuery] string id)
    {
      return Ok(await Integrations.DeleteById(id));
    }
  }
}
