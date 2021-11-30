using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace zero.Api.Modules.Countries;

public class CountriesController : ZeroApiController
{
  protected ICountryStore Store { get; set; }

  public CountriesController(ICountryStore store)
  {
    Store = store;
  }


  [HttpGet("empty")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual async Task<ActionResult<CountryEdit>> Empty()
  {
    throw new NotImplementedException("TEST ZERO");

    Country model = await Store.Empty();

    if (model == null)
    {
      return NotFound();
    }

    return Mapper.Map<Country, CountryEdit>(model);
  }


  [HttpGet("{id}")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<CountryEdit>> Get(string id, string changeVector = null)
  {
    Country model = await Store.Load(id, changeVector);

    if (model == null)
    {
      return NotFound();
    }

    return Mapper.Map<Country, CountryEdit>(model);
  }


  [HttpGet("")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Country> query)
  {
    query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
    Paged<Country> result = await Store.Load<zero_Backoffice_Countries_Listing>(query.Page, query.PageSize, q => q.Filter(query));

    return Mapper.Map<Country, CountryBasic>(result, (src, dest) =>
    {
      dest.Link = Url.Action(nameof(CountriesController.Get), new { id = dest.Id });
    });
  }


  [HttpPost("")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual async Task<ActionResult<Result>> Create(CountrySave saveModel)
  {
    Country model = Mapper.Map<CountrySave, Country>(saveModel);
    Result<Country> result = await Store.Create(model);

    if (result.IsSuccess)
    {
      Result<CountryEdit> mappedResult = Mapper.Map<Country, CountryEdit>(result);
      return CreatedAtAction(nameof(CountriesController.Get), new { id = model.Id }, mappedResult);
    }

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return Mapper.Map<Country, CountryEdit>(result);
  }


  [HttpPut("{id}")]
  [ZeroAuthorize(CountryPermissions.Update)]
  public virtual async Task<ActionResult<Result>> Update(string id, CountrySave updateModel)
  {
    if (id != updateModel.Id)
    {
      return BadRequest(BackofficeConstants.HttpErrors.NoIdMatchOnUpdate);
    }

    Country model = await Store.Load(id);

    if (model == null)
    {
      return BadRequest(BackofficeConstants.HttpErrors.IdNotFound);
    }

    Mapper.Map(updateModel, model);

    Result<Country> result = await Store.Update(model);

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    // TODO add Preference-Applied header, see https://github.com/microsoft/api-guidelines/blob/vNext/Guidelines.md#76-standard-response-headers
    return Mapper.Map<Country, CountryEdit>(result);
  }


  [HttpDelete("{id}")]
  [ZeroAuthorize(CountryPermissions.Delete)]
  public virtual async Task<ActionResult<Result>> Delete(string id)
  {
    Country model = await Store.Load(id);

    if (model == null)
    {
      return NotFound();
    }

    Result<Country> result = await Store.Delete(model);

    return result.WithoutModel();
  }
}