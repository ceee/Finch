using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Modules;

/// <summary>
/// | GET /zero/api/pages/empty
/// | GET /zero/api/pages/{id}
/// | GET /zero/api/pages/{id}/revisions
/// | GET /zero/api/pages[type,query,filter,...]
/// | POST /zero/api/pages
/// | PUT /zero/api/pages/{id}
/// | DELETE /zero/api/pages/{id}
/// </summary>
public class PagesController : ZeroBackofficeApiController
{
  protected IPagesStore Store { get; set; }

  public PagesController(IPagesStore store)
  {
    Store = store;
  }


  [HttpGet("empty")]
  public virtual async Task<Page> New()
  {
    return await Store.Empty();
  }

  [HttpGet("{id}")]
  public virtual async Task<Page> Get(string id, string changeVector = null)
  {
    return await Store.Load(id, changeVector);
  }

  [HttpGet]
  public virtual async Task<Paged<Page>> Get(ListQuery<Page> query)
  {
    return await Store.Load<zero_Backoffice_Countries_Listing>(query.Page, query.PageSize, q => q.OrderByDescending(x => x.CreatedDate));
  }

  //[HttpGet("{id}/revisions")]
  //public virtual async Task<List<Revision<Country>>> GetRevisions(string id)
  //{
  //  return await Collection.GetRevisions(id);
  //}


  [HttpPost]
  public virtual async Task<Result<Page>> Create(Country model)
  {
    return await Store.Create(model);
  }

  [HttpPut("{id}")]
  public virtual async Task<Result<Page>> Update(string id, Country model)
  {
    return await Store.Update(model);
  }

  [HttpDelete("{id}")]
  public virtual async Task<Result<Page>> Delete(string id)
  {
    return await Store.Delete(id);
  }
}