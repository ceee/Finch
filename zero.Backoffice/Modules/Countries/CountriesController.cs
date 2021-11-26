using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Modules.Countries;

public class CountriesController : ZeroBackofficeApiController
{
  protected ICountryStore Store { get; set; }

  public CountriesController(ICountryStore store)
  {
    Store = store;
  }


  [HttpGet("empty")]
  public virtual async Task<ActionResult<CountryDisplay>> Empty()
  {
    Country model = await Store.Empty();

    if (model == null)
    {
      return NotFound();
    }

    return Mapper.Map<Country, CountryDisplay>(model);
  }


  [HttpGet("{id}")]
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
  public virtual async Task<ActionResult<Paged>> Get(ListQuery<Country> query)
  {
    Paged<Country> result = await Store.Load<zero_Backoffice_Countries_Listing>(query.Page, query.PageSize, q => q.Filter(query));
    return Mapper.Map<Country, CountryBasic>(result);
  }


  [HttpPost]
  public virtual async Task<ActionResult<Result>> Create(CountrySave saveModel)
  {
    Country model = Mapper.Map<CountrySave, Country>(saveModel);
    Result<Country> result = await Store.Create(model);
    Result<CountryDisplay> mappedResult = Mapper.Map<Country, CountryDisplay>(result);

    if (result.IsSuccess)
    {
      return CreatedAtAction(nameof(CountriesController.Get), new { id = model.Id }, mappedResult);
    }

    return mappedResult;
  }


  [HttpPut("{id}")]
  public virtual async Task<ActionResult<Result>> Update(string id, CountrySave updateModel)
  {
    if (id != updateModel.Id)
    {
      return BadRequest(BackofficeConstants.HttpErrors.NoIdMatchOnUpdate);
    }

    Country model = Mapper.Map<CountrySave, Country>(updateModel);
    model.Id = id;
    Result<Country> result = await Store.Update(model);

    if (result.IsSuccess)
    {
      return NoContent();
    }

    return Mapper.Map<Country, CountryDisplay>(result);
  }


  [HttpDelete("{id}")]
  public virtual async Task<ActionResult<Result>> Delete(string id)
  {
    Country model = await Store.Load(id);

    if (model == null)
    {
      return NotFound();
    }

    Result<Country> result = await Store.Delete(model);

    if (result.IsSuccess)
    {
      return NoContent();
    }

    return result.WithoutModel();
  }
}