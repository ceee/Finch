using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace zero.Core
{
  public class Paths : IPaths
  {
    public string Root { get; set; }

    public string Media { get; set; }

    private bool IsDebug { get; set; }

    public const string MEDIA_FOLDER = "Uploads";

    private static char[] InvalidFilenameChars = null;

    private const char REPLACEMENT_CHAR = '-';

    private static Dictionary<char, string> Replacements = new Dictionary<char, string>()
    {
      { 'ä', "ae" },
      { 'ü', "ue" },
      { 'ö', "oe" },
      { 'ß', "ss" }
    };


    public Paths(string root, bool isDebug)
    {
      IsDebug = isDebug;
      Root = root;
      Media = Path.Combine(root, MEDIA_FOLDER);
    }


    /// <summary>
    /// Map a path
    /// </summary>
    public string Map(string path)
    {
      return Path.Combine(Root, path);
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
    string Root { get; set; }

    string Media { get; set; }

    /// <summary>
    /// Map a path
    /// </summary>
    string Map(string path);

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
