using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
using System.Text;

namespace Finch.FileStorage;

public class Paths : IPaths
{
  public string WebRoot { get; set; }

  public string ContentRoot { get; set; }

  public string SecureRoot { get; set; }

  public string Media { get; set; }

  protected IWebHostEnvironment Env { get; set; }

  public const string MEDIA_FOLDER = "Uploads";

  public const char PATH_SEPARATOR = '/';

  static char[] InvalidFilenameChars = null;

  const char REPLACEMENT_CHAR = '-';

  FileExtensionContentTypeProvider FileExtensionContentTypeProvider { get; }

  static readonly Dictionary<char, string> Replacements = new()
  {
    { 'ä', "ae" },
    { 'ü', "ue" },
    { 'ö', "oe" },
    { 'ß', "ss" }
  };


  public Paths(IWebHostEnvironment env)
  {
    Env = env;
    WebRoot = env.WebRootPath;
    ContentRoot = env.ContentRootPath;
    SecureRoot = Path.Combine(ContentRoot, "wwwroot.secure");
    Media = Path.Combine(WebRoot, MEDIA_FOLDER);
    FileExtensionContentTypeProvider = new();
  }


  /// <summary>
  /// Combine a path
  /// </summary>
  public string Combine(params string[] paths)
  {
    return Path.Combine(paths);
  }


  /// <summary>
  /// Map a path
  /// </summary>
  public string Map(string path)
  {
    return Path.Combine(WebRoot, path);
  }


  /// <summary>
  /// Map a secure path
  /// </summary>
  public string MapSecure(string path)
  {
    return Path.Combine(SecureRoot, path);
  }


  /// <summary>
  /// Map a path
  /// </summary>
  public string Map(params string[] paths)
  {
    return Path.Combine(WebRoot, Path.Combine(paths));
  }


  /// <summary>
  /// Map a secure path
  /// </summary>
  public string MapSecure(params string[] paths)
  {
    return Path.Combine(SecureRoot, Path.Combine(paths));
  }


  /// <summary>
  /// Create a directory if it does not exist yet
  /// </summary>
  public void Create(string directory)
  {
    if (!Directory.Exists(directory))
    {
      Directory.CreateDirectory(directory);
    }
  }


  /// <summary>
  /// Get content type for a filename
  /// </summary>
  public string GetContentType(string filename, string fallback = "application/octet-stream")
  {
    if (filename == null || !FileExtensionContentTypeProvider.TryGetContentType(Path.GetFileName(filename), out string contentType))
    {
      contentType = fallback;
    }
    return contentType;
  }


  /// <summary>
  /// Normalizes a filename and removes invalid chars
  /// </summary>
  public string ToFilename(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return value;
    }

    // lowercase for string
    value = value.ToLower();

    StringBuilder sb = new(value.Length);

    var invalids = InvalidFilenameChars ??= Path.GetInvalidFileNameChars();
    bool changed = false;

    for (int i = 0; i < value.Length; i++)
    {
      char c = value[i];

      if (Replacements.ContainsKey(c))
      {
        changed = true;
        sb.Append(Replacements[c]);
      }
      else if (invalids.Contains(c))
      {
        changed = true;
        sb.Append(REPLACEMENT_CHAR);
      }
      else
      {
        sb.Append(c);
      }
    }

    if (sb.Length == 0)
    {
      return "_";
    }

    return changed ? sb.ToString() : value;
  }
}


public interface IPaths
{
  string ContentRoot { get; set; }

  string WebRoot { get; set; }

  string SecureRoot { get; set; }

  string Media { get; set; }

  /// <summary>
  /// Combine a path
  /// </summary>
  string Combine(params string[] paths);

  /// <summary>
  /// Map a path
  /// </summary>
  string Map(string path);

  /// <summary>
  /// Map a secure path
  /// </summary>
  string MapSecure(string path);

  /// <summary>
  /// Map a path
  /// </summary>
  string Map(params string[] paths);

  /// <summary>
  /// Map a secure path
  /// </summary>
  string MapSecure(params string[] paths);

  /// <summary>
  /// Create a directory if it does not exist yet
  /// </summary>
  void Create(string directory);

  /// <summary>
  /// Get content type for a filename
  /// </summary>
  string GetContentType(string filename, string fallback = "application/octet-stream");

  /// <summary>
  /// Normalizes a filename and removes invalid chars
  /// </summary>
  string ToFilename(string value);
}