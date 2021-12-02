using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Countries;

public class CountriesController : ZeroApiEntityStoreController<Country, ICountryStore>
{
  public CountriesController(ICountryStore store) : base(store)
  {

  }


  [HttpGet("empty")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual Task<ActionResult<CountryEdit>> Empty(string flavor = null) => EmptyModel<CountryEdit>(flavor);


  [HttpGet("{id}")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual Task<ActionResult<CountryEdit>> Get(string id, string changeVector = null) => GetModel<CountryEdit>(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Country> query)
  {
    query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
    return GetModels<CountryBasic, zero_Api_Countries_Listing>(query);
  }


  [HttpPost("")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(CountrySave saveModel) => CreateModel<CountrySave, CountryEdit>(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(CountryPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, CountrySave updateModel, [FromQuery] string changeToken = null) => UpdateModel<CountrySave, CountryEdit>(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(CountryPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}