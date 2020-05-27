using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Mapper;
using zero.Core.Options;
using zero.Web.Filters;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  [CanEdit]
  [AddToken]
  [BackofficeGenericController]
  public abstract class BackofficeController : Controller, ISupportsGenericsController
  {
    IMapper _mapper;

    IZeroOptions _options;

    protected IMapper Mapper => _mapper ?? (_mapper = HttpContext?.RequestServices?.GetService<IMapper>());

    protected IZeroOptions Options => _options ?? (_options = HttpContext?.RequestServices?.GetService<IZeroOptions>());

    static JsonSerializerSettings JsonSettings;

    static JsonSerializerSettings TypedJsonSettings;


    static BackofficeController()
    {
      JsonSettings = new BackofficeJsonSerlializerSettings(false);
      TypedJsonSettings = new BackofficeJsonSerlializerSettings(true);
    }


    /// <summary>
    /// Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified data object to JSON.
    /// </summary>
    public override JsonResult Json(object data) => Json(data, false);


    /// <summary>
    /// Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified data object to JSON.
    /// </summary>
    public JsonResult Json(object data, bool typed) => Json(data, typed ? TypedJsonSettings : JsonSettings);


    /// <summary>
    /// Creates an edit model with appropriate options and permissions
    /// </summary>
    public JsonResult JsonEdit<T>(T data) => JsonEdit(data, false);


    /// <summary>
    /// Creates an edit model with appropriate options and permissions
    /// </summary>
    public JsonResult JsonEdit<T>(T data, bool typed)
    {
      return Json(new EditModel<T>()
      {
        Entity = data,
        CanEdit = true
      }, typed);
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

      if (result is ObsoleteEditModel)
      {
        ObsoleteEditModel editModel = result as ObsoleteEditModel;
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
