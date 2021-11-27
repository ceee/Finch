using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Modules.Countries;

public class CountriesController : ZeroBackofficeApiController
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
  public virtual async Task<ActionResult<CountryDisplay>> Empty()
  {
    Country model = await Store.Empty();

    if (model == null)
    {
      return NotFound();
    }

    return Mapper.Map<Country, CountryDisplay>(model);
  }


  [HttpGet("pick")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<Paged>> Pick(ListQuery<Country> query)
  {
    query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
    return await Picker.PickFrom(query.Page, query.PageSize, query);
  }


  [HttpGet("pick")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<List<PickerPreviewModel>>> Pick(string[] ids)
  {
    return await Picker.GetPreviews(ids);
  }


  [HttpGet("{id}")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<CountryDisplay>> Get(string id, string changeVector = null)
  {
    Country model = await Store.Load(id, changeVector);

    if (model == null)
    {
      return NotFound();
    }

    return Mapper.Map<Country, CountryDisplay>(model);
  }


  [HttpGet]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual async Task<ActionResult<Paged>> Get(ListQuery<Country> query)
  {
    query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
    Paged<Country> result = await Store.Load<zero_Backoffice_Countries_Listing>(query.Page, query.PageSize, q => q.Filter(query));
    return Mapper.Map<Country, CountryBasic>(result);
  }


  [HttpPost]
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

    return result.WithoutModel();
  }


  [HttpPut("{id}")]
  [ZeroAuthorize(CountryPermissions.Update)]
  public virtual async Task<ActionResult<Result>> Update(string id, CountrySave updateModel)
  {
    if (id != updateModel.Id)
    {
      return BadRequest(BackofficeConstants.HttpErrors.NoIdMatchOnUpdate);
    }

    Country model = Mapper.Map<CountrySave, Country>(updateModel);
    model.Id = id;
    Result<Country> result = await Store.Update(model);

    return result.WithoutModel();
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