using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Mapper;
using zero.Core.Options;
using zero.Core.Renderer;
using zero.Web.Filters;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  [BackofficeGenericController]
  public abstract class BackofficeController : Controller, ISupportsGenericsController
  {
    IMapper _mapper;
    IZeroOptions _options;
    IToken _token;

    protected IMapper Mapper => _mapper ?? (_mapper = HttpContext?.RequestServices?.GetService<IMapper>());
    protected IZeroOptions Options => _options ?? (_options = HttpContext?.RequestServices?.GetService<IZeroOptions>());
    protected IToken Token => _token ?? (_token = HttpContext?.RequestServices?.GetService<IToken>());

    static JsonSerializerSettings JsonSettings;
    static JsonSerializerSettings TypedJsonSettings;

    static Type AppAwareType = typeof(IAppAwareEntity);
    static Type AppAwareShareableType = typeof(IAppAwareShareableEntity);


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
    public JsonResult Edit<T>(T data, bool typed = true, Action<dynamic> transform = null) where T : IZeroEntity => Edit(data, null, typed, transform);


    /// <summary>
    /// Creates an edit model with appropriate options and permissions
    /// </summary>
    public JsonResult Edit<T>(T data, IRenderer<T> renderer, bool typed = true, Action<dynamic> transform = null) where T : IZeroEntity
    {
      Type type = typeof(T);
      bool canBeShared = AppAwareShareableType.IsAssignableFrom(type);

      //ControllerContext.ActionDescriptor.FilterDescriptors[0].

      dynamic model = new ExpandoObject();
      model.Entity = data;
      model.Renderer = renderer != null ? renderer.Build() : null;
      model.Token = Token.Get(data);
      model.IsAppAware = AppAwareType.IsAssignableFrom(type);
      model.CanBeShared = canBeShared;
      model.CanCreate = true;
      model.CanCreateShared = canBeShared;
      model.CanEdit = true;
      model.CanDelete = true;

      transform?.Invoke(model);

      return Json(model, typed);
    }


    public IActionResult JsonPreviews<T>(Dictionary<string, T> items, Func<T, PreviewModel> transform)
    {
      IList<PreviewModel> previews = new List<PreviewModel>();

      foreach (var item in items)
      {
        bool exists = item.Value != null;

        if (!exists)
        {
          previews.Add(new PreviewModel()
          {
            HasError = true,
            Icon = "fth-alert-circle color-red",
            Id = item.Key,
            Name = "@errors.preview.notfound",
            Text = "@errors.preview.notfound_text"
          });
        }
        else
        {
          previews.Add(transform(item.Value));
        }
      }

      return Json(previews);
    }



    public async Task<IActionResult> JsonPreviews<T>(Dictionary<string, T> items, Func<T, Task<PreviewModel>> transform)
    {
      IList<PreviewModel> previews = new List<PreviewModel>();

      foreach (var item in items)
      {
        bool exists = item.Value != null;

        if (!exists)
        {
          previews.Add(new PreviewModel()
          {
            HasError = true,
            Icon = "fth-alert-circle color-red",
            Id = item.Key,
            Name = "@errors.preview.notfound",
            Text = "@errors.preview.notfound_text"
          });
        }
        else
        {
          previews.Add(await transform(item.Value));
        }
      }

      return Json(previews);
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
