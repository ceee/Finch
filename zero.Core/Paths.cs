using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace zero.Core
{
  public class Paths : IPaths
  {
    public string WebRoot { get; set; }

    public string ContentRoot { get; set; }

    public string Media { get; set; }

    protected IWebHostEnvironment Env { get; set; }

    private bool IsDebug { get; set; }

    public const string MEDIA_FOLDER = "Uploads";

    public const char PATH_SEPARATOR = '/';

    private static char[] InvalidFilenameChars = null;

    private const char REPLACEMENT_CHAR = '-';

    private static Dictionary<char, string> Replacements = new Dictionary<char, string>()
    {
      { 'ä', "ae" },
      { 'ü', "ue" },
      { 'ö', "oe" },
      { 'ß', "ss" }
    };


    public Paths(IWebHostEnvironment env, bool isDebug)
    {
      Env = env;
      IsDebug = isDebug;
      WebRoot = env.WebRootPath;
      ContentRoot = env.ContentRootPath;
      Media = Path.Combine(WebRoot, MEDIA_FOLDER);
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
    /// Map a path
    /// </summary>
    public string Map(params string[] paths)
    {
      return Path.Combine(WebRoot, Path.Combine(paths));
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
    /// Normalizes a filename and removes invalid chars
    /// </summary>
    public string ToFilename(string value)
    {
      if (String.IsNullOrWhiteSpace(value))
      {
        return value;
      }

      // lowercase for string
      value = value.ToLower();

      StringBuilder sb = new StringBuilder(value.Length);

      var invalids = InvalidFilenameChars ?? (InvalidFilenameChars = Path.GetInvalidFileNameChars());
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
    /// Map a path
    /// </summary>
    string Map(params string[] paths);

    /// <summary>
    /// Create a directory if it does not exist yet
    /// </summary>
    void Create(string directory);

    /// <summary>
    /// Normalizes a filename and removes invalid chars
    /// </summary>
    string ToFilename(string value);
  }
}
