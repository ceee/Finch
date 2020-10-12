using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Web.Filters;
using zero.Web.Models;

namespace zero.Web.Controllers
{
  [ZeroAuthorize]
  [ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
  public abstract class BackofficeController : Controller
  {
    IZeroOptions _options;
    IToken _token;

    protected IZeroOptions Options => _options ?? (_options = HttpContext?.RequestServices?.GetService<IZeroOptions>());
    protected IToken Token => _token ?? (_token = HttpContext?.RequestServices?.GetService<IToken>());

    static Type AppAwareType = typeof(IAppAwareEntity);
    static Type AppAwareShareableType = typeof(IAppAwareShareableEntity);


    /// <summary>
    /// Creates a Microsoft.AspNetCore.Mvc.JsonResult object that serializes the specified data object to JSON.
    /// </summary>
    public JsonResult Json(object data, bool typed) => Json(data);


    /// <summary>
    /// Creates an edit model with appropriate options and permissions
    /// </summary>
    public IActionResult Edit<T>(T data, bool typed = true, Action<EditModel<T>> transform = null) where T : IZeroIdEntity
    {
      Type type = typeof(T);
      bool canBeShared = AppAwareShareableType.IsAssignableFrom(type);

      //ControllerContext.ActionDescriptor.FilterDescriptors[0].

      if (data == null)
      {
        return NotFound();
      }

      EditModel<T> model = new EditModel<T>();
      model.Entity = data;
      model.Meta.Token = Token.Get(data);
      model.Meta.IsAppAware = AppAwareType.IsAssignableFrom(type);
      model.Meta.CanBeShared = canBeShared;
      model.Meta.CanCreate = true;
      model.Meta.CanCreateShared = canBeShared;
      model.Meta.CanEdit = true;
      model.Meta.CanDelete = true;

      transform?.Invoke(model);

      return Json(model, typed);
    }


    /// <summary>
    /// Creates an edit model with appropriate options and permissions
    /// </summary>
    public IActionResult Edit<T, TWrapper>(TWrapper data, bool typed = true, Action<EditModel<T>> transform = null) 
      where T : IZeroIdEntity
      where TWrapper : EditModel<T>, new() 
    {
      Type type = typeof(T);
      bool canBeShared = AppAwareShareableType.IsAssignableFrom(type);

      //ControllerContext.ActionDescriptor.FilterDescriptors[0].

      data.Meta.Token = Token.Get(data.Entity);
      data.Meta.IsAppAware = AppAwareType.IsAssignableFrom(type);
      data.Meta.CanBeShared = canBeShared;
      data.Meta.CanCreate = true;
      data.Meta.CanCreateShared = canBeShared;
      data.Meta.CanEdit = true;
      data.Meta.CanDelete = true;

      transform?.Invoke(data);

      return Json(data, typed);
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


    protected IActionResult Download(Stream stream, string filename)
    {
      if (stream == null)
      {
        // TODO add success property + return error response
      }

      if (filename.Contains("{date}"))
      {
        filename = filename.Replace("{date}", DateTimeOffset.Now.ToString("yyyy-MM-dd"));
      }

      var provider = new FileExtensionContentTypeProvider();
      if (filename == null || !provider.TryGetContentType(Path.GetFileName(filename), out string contentType))
      {
        contentType = "application/octet-stream";
      }

      Response.Headers.Add("zero-filename", filename);

      return base.File(stream, contentType, filename, true);
    }
  }
}
