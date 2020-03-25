using Microsoft.AspNetCore.Mvc;
using zero.Core;

namespace zero.Web.Controllers
{
  public abstract class BackofficeController : Controller
  {
    protected IZeroConfiguration Configuration { get; set; }


    public BackofficeController(IZeroConfiguration config)
    {
      Configuration = config;
    }
  }
}
