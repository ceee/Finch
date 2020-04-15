using Microsoft.AspNetCore.Mvc;
using zero.Core;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  public abstract class BackofficeController : Controller
  {
    protected IZeroConfiguration Configuration { get; set; }


    public BackofficeController(IZeroConfiguration config)
    {
      Configuration = config;
    }
  }
}
