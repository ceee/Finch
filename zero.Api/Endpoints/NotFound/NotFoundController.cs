using Microsoft.AspNetCore.Mvc;

namespace zero.Api.Endpoints.NotFound;

public class NotFoundZeroApiController : Controller
{
  public virtual ActionResult Index()
  {
    return NotFound();
  }
}