using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace zero.Debug.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet]
    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> Test()
    {
      await Task.Delay(3000);
      return Json(new
      {
        result = "okay"
      });
    }
  }
}