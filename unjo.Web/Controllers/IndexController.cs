using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace unjo.Web.Controllers
{
  [AllowAnonymous]
  public class IndexController : BackofficeController
  {
    public IActionResult Index()
    {
      return View("/Index.cshtml");
    }
  }
}
