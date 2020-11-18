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
  [ApiController]
  [Route("getsreplaced/[controller]/[action]")]
  public abstract class BackofficeController : ControllerBase
  {
    IZeroOptions _options;
    IToken _token;

    protected IZeroOptions Options => _options ?? (_options = HttpContext?.RequestServices?.GetService<IZeroOptions>());
    protected IToken Token => _token ?? (_token = HttpContext?.RequestServices?.GetService<IToken>());


    /// <summary>
    /// Creates an edit model with appropriate options and permissions
    /// </summary>
    public EditModel<T> Edit<T>(T data, bool typed = true, Action<EditModel<T>> transform = null) where T : IZeroIdEntity
    {
      Type type = typeof(T);

      //ControllerContext.ActionDescriptor.FilterDescriptors[0].

      if (data == null)
      {
        return null;
      }

      EditModel<T> model = new EditModel<T>();
      model.Entity = data;
      model.Meta.Token = Token.Get(data);
      //model.Meta.IsAppAware = AppAwareType.IsAssignableFrom(type); // TODO appx
      //model.Meta.CanBeShared = canBeShared;
      model.Meta.CanCreate = true;
      //model.Meta.CanCreateShared = canBeShared;
      model.Meta.CanEdit = true;
      model.Meta.CanDelete = true;

      transform?.Invoke(model);

      return model;
    }


    /// <summary>
    /// Creates an edit model with appropriate options and permissions
    /// </summary>
    public TWrapper Edit<T, TWrapper>(TWrapper data, bool typed = true, Action<EditModel<T>> transform = null) 
      where T : IZeroIdEntity
      where TWrapper : EditModel<T>, new() 
    {
      Type type = typeof(T);

      //ControllerContext.ActionDescriptor.FilterDescriptors[0].

      data.Meta.Token = Token.Get(data.Entity);
      //data.Meta.IsAppAware = AppAwareType.IsAssignableFrom(type); // TODO appx
      //data.Meta.CanBeShared = canBeShared;
      data.Meta.CanCreate = true;
      //data.Meta.CanCreateShared = canBeShared;
      data.Meta.CanEdit = true;
      data.Meta.CanDelete = true;

      transform?.Invoke(data);

      return data;
    }


    public IList<PreviewModel> Previews<T>(Dictionary<string, T> items, Func<T, PreviewModel> transform)
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

      return previews;
    }



    public async Task<IList<SelectModel>> SelectList<T>(IAsyncEnumerable<T> enumerable) where T : IZeroEntity
    {
      List<SelectModel> items = new List<SelectModel>();

      await foreach (T item in enumerable)
      {
        items.Add(new SelectModel()
        {
          Id = item.Id,
          Name = item.Name,
          IsActive = item.IsActive
        });
      }

      return items;
    }



    public async Task<IList<PreviewModel>> Previews<T>(Dictionary<string, T> items, Func<T, Task<PreviewModel>> transform)
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

      return previews;
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
