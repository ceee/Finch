using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Finch.Media.ImageSharp;

/// <summary>
/// Returns images stored in the local physical file system.
/// </summary>
public class PhysicalFileProvider : IImageProvider
{
  const string Prefix = "/:";

  /// <summary> 
  /// The file provider abstraction.
  /// </summary>
  readonly IFileProvider _fileProvider;

  /// <summary>
  /// Contains various format helper methods based on the current configuration.
  /// </summary>
  readonly FormatUtilities _formatUtilities;

  /// <summary>
  /// Initializes a new instance of the <see cref="PhysicalFileSystemProvider"/> class.
  /// </summary>
  /// <param name="environment">The environment used by this middleware.</param>
  /// <param name="formatUtilities">Contains various format helper methods based on the current configuration.</param>
  public PhysicalFileProvider(IWebHostEnvironment environment, FormatUtilities formatUtilities)
  {
    this._fileProvider = environment.WebRootFileProvider;
    this._formatUtilities = formatUtilities;
  }

  /// <inheritdoc/>
  public ProcessingBehavior ProcessingBehavior { get; } = ProcessingBehavior.CommandOnly;

  /// <inheritdoc/>
  public Func<HttpContext, bool> Match { get; set; } = _ => true;

  /// <inheritdoc/>
  public bool IsValidRequest(HttpContext context)
  {
    string displayUrl = context.Request.GetDisplayUrl();

    if (!_formatUtilities.TryGetExtensionFromUri(displayUrl, out string extension))
    {
      return false;
    }

    return displayUrl.Contains(Prefix);
  }

  /// <inheritdoc/>
  public Task<IImageResolver> GetAsync(HttpContext context)
  {
    string path = GetPath(context);

    // Path has already been correctly parsed before here.
    IFileInfo fileInfo = this._fileProvider.GetFileInfo(path);

    // Check to see if the file exists.
    if (!fileInfo.Exists)
    {
      return Task.FromResult<IImageResolver>(null);
    }

    return Task.FromResult<IImageResolver>(new FileProviderImageResolver(fileInfo));
  }


  string GetPath(HttpContext context)
  {
    string path = context.Request.Path.Value;
    List<string> parts = path.Split('/').ToList();

    parts.RemoveAt(parts.Count - 2);
    return string.Join('/', parts);
  }
}