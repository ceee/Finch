using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

namespace zero.Backoffice.Controllers;

public static class ZeroBackofficeControllerExtensions
{
  /// <summary>
  /// Transform entities to a preview list
  /// </summary>
  public static List<PickerPreviewModel> TransformToPreviewModels<T>(this ZeroBackofficeController controller, Dictionary<string, T> items, Action<T, PickerPreviewModel> transform = null) where T : ZeroIdEntity
  {
    List<PickerPreviewModel> previews = new();

    foreach (var item in items)
    {
      if (item.Value == null)
      {
        previews.Add(new PickerPreviewModel()
        {
          HasError = true,
          Icon = "fth-alert-circle color-red",
          Id = item.Key,
          Name = "@errors.preview.notfound",
          Text = "@errors.preview.notfound_text"
        });
        continue;
      }

      PickerPreviewModel model = new() { Id = item.Value.Id };

      if (item.Value is ZeroEntity)
      {
        model.Name = (item.Value as ZeroEntity).Name;
      }

      transform?.Invoke(item.Value, model);
      previews.Add(model);
    }

    return previews;
  }


  /// <summary>
  /// Transform entities to a select list
  /// </summary>
  public static List<PickerModel> TransformToSelectModels<T>(this ZeroBackofficeController controller, IEnumerable<T> enumerable, Action<T, PickerModel> transform = null) where T : ZeroIdEntity
  {
    List<PickerModel> items = new();

    foreach (T item in enumerable)
    {
      PickerModel model = new()
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


  /// <summary>
  /// Provides a file stream for download in the browser
  /// </summary>
  public static IActionResult DownloadFile(this ZeroBackofficeController controller, Stream stream, string filename)
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

    controller.Response.Headers.Add("zero-filename", filename);

    return controller.File(stream, contentType, filename, true);
  }


  /// <summary>
  /// Provides a file stream to the browser
  /// </summary>
  public static IActionResult File(this ZeroBackofficeController controller, FileStorage.FileResult file)
  {
    if (file == null)
    {
      return controller.NotFound();
    }

    FileStream stream = System.IO.File.OpenRead(file.Path);
    return controller.File(stream, file.ContentType, file.Filename);
  }
}
