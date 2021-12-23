using Microsoft.AspNetCore.Mvc;
using System.Collections;
using zero.Mapper;

namespace zero.Api.Endpoints.Integrations;

public class IntegrationsController : ZeroApiController
{
  readonly IIntegrationTypeService IntegrationTypes;
  readonly IIntegrationStore Store;

  public IntegrationsController(IIntegrationTypeService integrationTypes, IIntegrationStore store)
  {
    IntegrationTypes = integrationTypes;
    Store = store;
  }



  [HttpGet("types")]
  //[ZeroAuthorize(SpacePermissions.Read)]
  public virtual async Task<ActionResult<IEnumerable>> GetTypes()
  {
    IEnumerable<IntegrationTypeDisplay> result = Mapper.Map<IntegrationType, IntegrationTypeDisplay>(IntegrationTypes.GetAll()).ToList();

    foreach (IntegrationTypeDisplay display in result)
    {
      Integration model = await Store.Load(display.Alias);
      
      if (model != null)
      {
        display.IsConfigured = true;
        display.IsActivated = model.IsActive;
        display.ModelId = model.Id;
      }
    }

    return Ok(result);
  }


  [HttpGet("types/{alias}")]
  //[ZeroAuthorize(SpacePermissions.Read)]
  public virtual async Task<ActionResult<IntegrationTypeDisplay>> GetType(string alias)
  {
    IntegrationTypeDisplay result = Mapper.Map<IntegrationType, IntegrationTypeDisplay>(IntegrationTypes.GetByAlias(alias));

    if (result == null)
    {
      return NotFound();
    }

    Integration model = await Store.Load(result.Alias);

    if (model != null)
    {
      result.IsConfigured = true;
      result.IsActivated = model.IsActive;
      result.ModelId = model.Id;
    }

    return Ok(result);
  }


  [HttpGet("empty/{alias}")]
  //[ZeroAuthorize(CountryPermissions.Create)]
  public virtual async Task<ActionResult<Integration>> Empty(string alias)
  {
    Integration model = await Store.Empty(alias);

    if (model == null)
    {
      return NotFound();
    }

    return model;
  }


  [HttpGet("{alias}")]
  //[ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<Integration>> Get(string alias)
  {
    Integration model = await Store.Load(alias);

    if (model == null)
    {
      return NotFound();
    }

    HttpContext.Items[ApiConstants.ChangeToken] = Store.GetChangeToken(model);

    return model;
  }


  [HttpPost("")]
  //[ZeroAuthorize(CountryPermissions.Create)]
  public virtual async Task<ActionResult<Result>> Create(Integration saveModel)
  {
    Result<Integration> result = await Store.Create(saveModel);

    bool minimalResponse = Hints.ResponsePreference == ApiResponsePreference.Minimal;

    if (result.IsSuccess)
    {
      return Created("/", minimalResponse ? null : saveModel);
    }

    return result.WithoutModel();
  }


  [HttpPut("{alias}")]
  //[ZeroAuthorize(CountryPermissions.Update)]
  public virtual async Task<ActionResult<Result>> Update(string alias, Integration updateModel, [FromQuery] string changeToken = null)
  {
    if (alias != updateModel.TypeAlias)
    {
      return BadRequest(Result.Fail(nameof(alias), "@integration.errors.noaliasmatch"));
    }

    Result<Integration> result = await Store.Update(updateModel);

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return result;
  }


  [HttpPut("{alias}/activate")]
  //[ZeroAuthorize(CountryPermissions.Update)]
  public virtual async Task<ActionResult<Result>> Activate(string alias)
  {
    Result<Integration> result = await Store.Activate(alias);

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return result;
  }


  [HttpPut("{alias}/deactivate")]
  //[ZeroAuthorize(CountryPermissions.Update)]
  public virtual async Task<ActionResult<Result>> Deactivate(string alias)
  {
    Result<Integration> result = await Store.Deactivate(alias);

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return result;
  }


  [HttpDelete("{alias}")]
  //[ZeroAuthorize(CountryPermissions.Delete)]
  public virtual async Task<ActionResult<Result>> Delete(string alias)
  {
    Result<Integration> result = await Store.Delete(alias);
    return result.WithoutModel();
  }
}