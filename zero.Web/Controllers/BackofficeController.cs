using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Web.Mapper;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
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

      return Json(result);
    }


    protected IActionResult As<T, TTarget>(IEnumerable<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      IList<TTarget> result = new List<TTarget>();

      foreach (T item in model)
      {
        result.Add(Mapper.Map<T, TTarget>(item));
      }

      return Json(result);
    }


    protected IActionResult As<T, TTarget>(ListResult<T> model) where TTarget : class, new() where T : IZeroEntity
    {
      if (model == null)
      {
        return new StatusCodeResult(404);
      }

      IList<TTarget> list = new List<TTarget>();

      foreach (T item in model.Items)
      {
        list.Add(Mapper.Map<T, TTarget>(item));
      }

      return Json(new ListResult<TTarget>(list, model.TotalItems, model.Page, model.PageSize)
      {
        Statistics = model.Statistics
      });
    }


    protected TTarget Map<T, TTarget>(T model) where TTarget : class, new()
    {
      return Mapper.Map<T, TTarget>(model);
    }
  }
}
