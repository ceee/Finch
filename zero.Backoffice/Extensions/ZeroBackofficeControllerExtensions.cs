using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using zero.Api.Abstractions;

namespace zero.Backoffice.Extensions;

public static class ZeroBackofficeControllerExtensions
{
  /// <summary>
  /// Provides a file stream for download in the browser
  /// </summary>
  public static IActionResult DownloadFile(this ZeroApiController controller, Stream stream, string filename)
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
  public static IActionResult File(this ZeroApiController controller, FileStorage.FileResult file)
  {
    if (file == null)
    {
      return controller.NotFound();
    }

    FileStream stream = System.IO.File.OpenRead(file.Path);
    return controller.File(stream, file.ContentType, file.Filename);
  }
}
