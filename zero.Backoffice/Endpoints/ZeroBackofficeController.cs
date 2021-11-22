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

namespace zero.Backoffice;

[ZeroAuthorize]
//[ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
[ApiController]
[Route("getsreplaced/[controller]/[action]")]
//[ServiceFilter(typeof(BackofficeFilterAttribute))]
public abstract class ZeroBackofficeController : ControllerBase
{
  IZeroOptions _options;

  public bool IsCoreDatabase { get; protected set; }

  protected IZeroOptions Options => _options ?? (_options = HttpContext?.RequestServices?.GetService<IZeroOptions>());


  /// <summary>
  /// Is execuated when the scope changes.
  /// The scope is evaluated by the BackofficeFilterAttribute.
  /// </summary>
  public virtual void OnScopeChanged(string scope) { }


  /// <summary>
  /// Creates an edit model with appropriate options and permissions
  /// </summary>
  public EditModel<T> Edit<T>(T data, bool typed = true, Action<EditModel<T>> transform = null) where T : ZeroIdEntity
  {
    Type type = typeof(T);

    //ControllerContext.ActionDescriptor.FilterDescriptors[0].

    if (data == null)
    {
      return null;
    }

    EditModel<T> model = new EditModel<T>();
    model.Entity = data;
    model.Meta.Token = null; // Token.Get(data);
    //model.Meta.IsAppAware = AppAwareType.IsAssignableFrom(type); // TODO appx
    //model.Meta.CanBeShared = canBeShared;
    model.Meta.CanCreate = true;
    //model.Meta.CanCreateShared = canBeShared;
    model.Meta.CanEdit = true;
    model.Meta.CanDelete = true;
    model.Meta.IsShared = IsCoreDatabase;

    transform?.Invoke(model);

    return model;
  }


  /// <summary>
  /// Creates an edit model with appropriate options and permissions
  /// </summary>
  public TWrapper Edit<T, TWrapper>(TWrapper data, bool typed = true, Action<EditModel<T>> transform = null) 
    where T : ZeroIdEntity
    where TWrapper : EditModel<T>, new() 
  {
    Type type = typeof(T);

    //ControllerContext.ActionDescriptor.FilterDescriptors[0].

    data.Meta.Token = null; // Token.Get(data.Entity);
    //data.Meta.IsAppAware = AppAwareType.IsAssignableFrom(type); // TODO appx
    //data.Meta.CanBeShared = canBeShared;
    data.Meta.CanCreate = true;
    //data.Meta.CanCreateShared = canBeShared;
    data.Meta.CanEdit = true;
    data.Meta.CanDelete = true;
    data.Meta.IsShared = IsCoreDatabase;

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


  public IList<PreviewModel> Previews<T>(Dictionary<string, T> items, Action<T, PreviewModel> transform = null) where T : ZeroIdEntity
  {
    IList<PreviewModel> previews = new List<PreviewModel>();

    foreach (var item in items)
    {
      if (item.Value == null)
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
        PreviewModel model = new()
        {
          Id = item.Value.Id
        };

        if (item.Value is ZeroEntity)
        {
          model.Name = (item.Value as ZeroEntity).Name;
        }

        transform?.Invoke(item.Value, model);

        previews.Add(model);
      }
    }

    return previews;
  }



  public async Task<IList<SelectModel>> SelectList<T>(IAsyncEnumerable<T> enumerable, Action<T, SelectModel> transform = null) where T : ZeroIdEntity
  {
    List<SelectModel> items = new List<SelectModel>();

    await foreach (T item in enumerable)
    {
      SelectModel model = new()
      {
        Id = item.Id
      };

      if (item is ZeroEntity)
      {
        model.Name = (item as ZeroEntity).Name;
        model.IsActive = (item as ZeroEntity).IsActive;
      }

      transform?.Invoke(item, model);

      items.Add(model);
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


  protected IActionResult File(Core.Entities.FileResult file)
  {
    if (file == null)
    {
      return NotFound();
    }

    FileStream stream = System.IO.File.OpenRead(file.Path);
    return File(stream, file.ContentType, file.Filename);
  }
}