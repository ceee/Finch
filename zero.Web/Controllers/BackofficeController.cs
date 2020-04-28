using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Filters;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize, CanEdit, AddToken]
  public abstract class BackofficeController : Controller
  {
    protected IZeroConfiguration Configuration { get; set; }

    protected IMapper Mapper { get; set; }

    protected IToken Token { get; set; }


    public BackofficeController(IZeroConfiguration config, IMapper mapper, IToken token)
    {
      Configuration = config;
      Mapper = mapper;
      Token = token;
    }


    public BackofficeController(IZeroConfiguration config)
    {
      Configuration = config;
    }


    protected IActionResult As<T, TTarget>(T model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      if (Mapper == null)
      {
        // TODO show error with help on how to inject mapper in constructor + base constructor
      }

      TTarget result = Mapper.Map<T, TTarget>(model);

      if (result is EditModel)
      {
        EditModel editModel = result as EditModel;
        //model.CanEdit = 
      }

      return Json(result);
    }


    protected IActionResult As<T, TTarget>(IEnumerable<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      return Json(Mapper.Map<T, TTarget>(model));
    }


    protected IActionResult As<T, TTarget>(ListResult<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      return Json(Mapper.Map<T, TTarget>(model));
    }

    protected IActionResult As<T, TTarget>(EntityResult<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      return Json(Mapper.Map<T, TTarget>(model));
    }

    protected TTarget Map<T, TTarget>(T model) where TTarget : class, new()
    {
      return Mapper.Map<T, TTarget>(model);
    }
  }
}
