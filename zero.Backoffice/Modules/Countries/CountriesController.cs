using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Modules;

/// <summary>
/// | GET /zero/api/countries/empty
/// | GET /zero/api/countries/{id}
/// | GET /zero/api/countries/{id}/revisions
/// | GET /zero/api/countries[type,query,filter,...]
/// | POST /zero/api/countries
/// | PUT /zero/api/countries/{id}
/// | DELETE /zero/api/countries/{id}
/// </summary>
public class CountriesController : ZeroBackofficeApiController
{
  protected ICountriesCollection Collection { get; set; }

  public CountriesController(ICountriesCollection collection)
  {
    Collection = collection;
  }


  [HttpGet("empty")]
  public virtual async Task<Country> New()
  {
    return await Collection.Empty();
  }

  [HttpGet("{id}")]
  public virtual async Task<Country> Get(string id, string changeVector = null)
  {
    return await Collection.Load(id, changeVector);
  }

  [HttpGet]
  public virtual async Task<Paged<Country>> Get(ListBackofficeQuery<Country> query)
  {
    return await Collection.Load<zero_Backoffice_Countries_Listing>(query.Page, query.PageSize, q => q.OrderByDescending(x => x.CreatedDate));
  }

  //[HttpGet("{id}/revisions")]
  //public virtual async Task<List<Revision<Country>>> GetRevisions(string id)
  //{
  //  return await Collection.GetRevisions(id);
  //}


  [HttpPost]
  public virtual async Task<EntityResult<Country>> Create(Country country)
  {
    return await Collection.Create(country);
  }

  [HttpPut("{id}")]
  public virtual async Task<EntityResult<Country>> Update(string id, Country country)
  {
    return await Collection.Update(country);
  }

  [HttpDelete("{id}")]
  public virtual async Task<EntityResult<Country>> Delete(string id)
  {
    return await Collection.Delete(id);
  }
}