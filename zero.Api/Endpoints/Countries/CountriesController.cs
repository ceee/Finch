using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.Countries;

public class CountriesController : ZeroApiEntityStoreController<Country, ICountryStore>
{
  public CountriesController(ICountryStore store) : base(store) { }


  [HttpGet("empty")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual Task<ActionResult<Country>> Empty(string flavor = null) => EmptyModel(flavor);


  [HttpGet("{id}")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual Task<ActionResult<Country>> Get(string id, string changeVector = null) => GetModel(id, changeVector);


  [HttpGet("")]
  [ZeroAuthorize(CountryPermissions.Read)]
  public virtual Task<ActionResult<Paged>> Get([FromQuery] ListQuery<Country> query)
  {
    query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
    return GetModelsByIndex<CountryBasic, zero_Api_Countries_Listing>(query);
  }


  [HttpPost("")]
  [ZeroAuthorize(CountryPermissions.Create)]
  public virtual Task<ActionResult<Result>> Create(Country saveModel) => CreateModel(saveModel);


  [HttpPut("{id}")]
  [ZeroAuthorize(CountryPermissions.Update)]
  public virtual Task<ActionResult<Result>> Update(string id, Country updateModel, [FromQuery] string changeToken = null) => UpdateModel(id, updateModel, changeToken);


  [HttpDelete("{id}")]
  [ZeroAuthorize(CountryPermissions.Delete)]
  public virtual Task<ActionResult<Result>> Delete(string id) => DeleteModel(id);
}