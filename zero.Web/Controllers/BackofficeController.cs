using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Identity;
using zero.Web.Mapper;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  public abstract class BackofficeController : Controller
  {
    protected IZeroConfiguration Configuration { get; set; }

    protected IMapper Mapper { get; set; }


    public BackofficeController(IZeroConfiguration config, IMapper mapper)
    {
      Configuration = config;
      Mapper = mapper;
    }


    public BackofficeController(IZeroConfiguration config)
    {
      Configuration = config;
    }


    protected IActionResult Json<T, TTarget>(T model) where TTarget : class, new()
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      return Json(Mapper.Map<T, TTarget>(model));
    }
  }
}
