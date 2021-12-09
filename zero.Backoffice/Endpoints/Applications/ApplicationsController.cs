using Microsoft.AspNetCore.Mvc;
using zero.Api.Filters;

namespace zero.Backoffice.Endpoints.Applications;

[ZeroSystemApi]
public class ApplicationsController : ZeroBackofficeController
{
  readonly IApplicationStore Store;

  public ApplicationsController(IApplicationStore store)
  {
    Store = store;
  }

  [HttpGet("")]
  public virtual async Task<ActionResult<Paged>> Get()
  {
    Paged<Application> result = await Store.Load(1, 100);
    return Mapper.Map<Application, ApplicationPresentation>(result);
  }
}