using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace zero.Api.Modules.Countries;

public class CountriesController : ZeroApiController
{
  protected ICountryStore Store { get; set; }

  protected IPickerProvider<Country> Picker { get; set; }


  public CountriesController(ICountryStore store, IPickerProvider<Country> picker)
  {
    Store = store;
    Picker = picker;
  }


  [HttpGet("empty")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual async Task<ActionResult<DisplayModel>> Empty()
  {
    throw new NotImplementedException("TEST ZERO");

    Country model = await Store.Empty();

    if (model == null)
    {
      return NotFound();
    }

    return Mapper.Map<Country, CountryDisplay>(model);
  }


  [HttpGet("pick")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<Paged>> Pick([FromQuery] ListQuery<Country> query)
  {
    query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
    Paged<Country> result = await Store.Load<zero_Backoffice_Countries_Listing>(query.Page, query.PageSize, q => q.Filter(query));
    return Mapper.Map<Country, PickerModel>(result);
  }


  [HttpGet("pickpreview")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<IEnumerable>> Pick([FromQuery] string[] ids)
  {
    Dictionary<string, Country> model = await Store.Load(ids);
    return Mapper.Map<Country, PickerPreviewModel>(model);
  }


  [HttpGet("{id}")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<DisplayModel>> Get(string id, string changeVector = null)
  {
    Country model = await Store.Load(id, changeVector);

    if (model == null)
    {
      return NotFound();
    }

    return Mapper.Map<Country, CountryDisplay>(model);
  }


  [HttpGet("")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Country> query)
  {
    query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
    Paged<Country> result = await Store.Load<zero_Backoffice_Countries_Listing>(query.Page, query.PageSize, q => q.Filter(query));
    return Mapper.Map<Country, CountryBasic>(result);
  }


  [HttpPost("")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual async Task<ActionResult<Result>> Create(CountrySave saveModel)
  {
    Country model = Mapper.Map<CountrySave, Country>(saveModel);
    Result<Country> result = await Store.Create(model);

    if (result.IsSuccess)
    {
      Result<CountryDisplay> mappedResult = Mapper.Map<Country, CountryDisplay>(result);
      return CreatedAtAction(nameof(CountriesController.Get), new { id = model.Id }, mappedResult);
    }

    if (Hints.ResponsePreference == ApiResponsePreference.Minimal)
    {
      return result.WithoutModel();
    }

    return Mapper.Map<Country, CountryDisplay>(result);
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
    return Mapper.Map<Country, CountryDisplay>(result);
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