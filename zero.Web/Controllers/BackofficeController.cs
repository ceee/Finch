using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Mapper;
using zero.Web.Filters;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize, CanEdit, AddToken]
  public abstract class BackofficeController : Controller
  {
    IMapper _mapper;

    IZeroOptions _options;

    protected IMapper Mapper
    {
      get
      {
        return _mapper ?? (_mapper = HttpContext?.RequestServices?.GetService<IMapper>());
      }
    }

    protected IZeroOptions Options
    {
      get
      {
        return _options ?? (_options = HttpContext?.RequestServices?.GetService<IZeroOptions>());
      }
    }


    public BackofficeController()
    {
      
    }


    protected async Task<IActionResult> As<T, TTarget>(T model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      if (Mapper == null)
      {
        // TODO show error with help on how to inject mapper in constructor + base constructor
      }

      TTarget result = await Mapper.Map<T, TTarget>(model);

      if (result is EditModel)
      {
        EditModel editModel = result as EditModel;
        //model.CanEdit = 
      }

      return Json(result);
    }


    protected async Task<IActionResult> As<T, TTarget>(IEnumerable<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      return Json(await Mapper.Map<T, TTarget>(model));
    }


    protected async Task<IActionResult> As<T, TTarget>(ListResult<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      return Json(await Mapper.Map<T, TTarget>(model));
    }

    protected async Task<IActionResult> As<T, TTarget>(EntityResult<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      return Json(await Mapper.Map<T, TTarget>(model));
    }

    protected async Task<TTarget> Map<T, TTarget>(T model) where TTarget : class, new()
    {
      return await Mapper.Map<T, TTarget>(model);
    }
  }
}
