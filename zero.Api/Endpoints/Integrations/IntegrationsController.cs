using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace zero.Api.Endpoints.Integrations;

public class IntegrationsController : ZeroApiController
{
  readonly IIntegrationTypeService IntegrationTypes;

  public IntegrationsController(IIntegrationTypeService integrationTypes)
  {
    IntegrationTypes = integrationTypes;
  }


  [HttpGet("types")]
  //[ZeroAuthorize(SpacePermissions.Read)]
  public virtual ActionResult<IEnumerable> GetTypes()
  {
    IEnumerable<IntegrationTypeDisplay> result = Mapper.Map<IntegrationType, IntegrationTypeDisplay>(IntegrationTypes.GetAll());
    return Ok(result);
  }

  [HttpGet("types/{alias}")]
  //[ZeroAuthorize(SpacePermissions.Read)]
  public virtual ActionResult<SpaceType> GetTypes(string alias)
  {
    IntegrationTypeDisplay result = Mapper.Map<IntegrationType, IntegrationTypeDisplay>(IntegrationTypes.GetByAlias(alias));
    return Ok(result);
  }
}