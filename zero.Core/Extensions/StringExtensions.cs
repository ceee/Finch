using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace zero.Core.Extensions
{
  public static class StringExtensions
  {
    const string SPACE = " ";

    static Regex replaceMultipleSpacesRegex { get; } = new Regex("[ ]{2,}", RegexOptions.None);

    static Regex newLineCharsRegex { get; } = new Regex("\t|\n|\r", RegexOptions.None);


    public static string FullTrim(this string value)
    {
      if (String.IsNullOrEmpty(value))
      {
        return value;
      }

      return replaceMultipleSpacesRegex.Replace(value, SPACE).Trim();
    }


    public static string EnsureStartsWith(this string input, string toStartWith)
    {
      if (input.StartsWith(toStartWith)) return input;
      return toStartWith + input.TrimStart(toStartWith);
    }


    public static string EnsureStartsWith(this string input, char value)
    {
      return input.StartsWith(value.ToString(CultureInfo.InvariantCulture)) ? input : value + input;
    }


    public static string EnsureEndsWith(this string input, char value)
    {
      return input.EndsWith(value.ToString(CultureInfo.InvariantCulture)) ? input : input + value;
    }


    public static string EnsureEndsWith(this string input, string toEndWith)
    {
      return input.EndsWith(toEndWith.ToString(CultureInfo.InvariantCulture)) ? input : input + toEndWith;
    }


    public static string EnsureSurroundedWith(this string input, char toSurroundWith)
    {
      return input.EnsureStartsWith(toSurroundWith).EnsureEndsWith(toSurroundWith);
    }


    public static string Or(this string value, string fallback)
    {
      return String.IsNullOrWhiteSpace(value) ? fallback : value;
    }


    public static string TrimEnd(this string value, string forRemoving)
    {
      if (String.IsNullOrEmpty(value)) return value;
      if (String.IsNullOrEmpty(forRemoving)) return value;

      while (value.EndsWith(forRemoving, StringComparison.InvariantCultureIgnoreCase))
      {
        value = value.Remove(value.LastIndexOf(forRemoving, StringComparison.InvariantCultureIgnoreCase));
      }
      return value;
    }


    public static string TrimStart(this string value, string forRemoving)
    {
      if (String.IsNullOrEmpty(value)) return value;
      if (String.IsNullOrEmpty(forRemoving)) return value;

      while (value.StartsWith(forRemoving, StringComparison.InvariantCultureIgnoreCase))
      {
        value = value.Substring(forRemoving.Length);
      }
      return value;
    }


    public static bool HasValue(this string input)
    {
      return !String.IsNullOrWhiteSpace(input);
    }


    public static bool IsNullOrEmpty(this string input)
    {
      return String.IsNullOrEmpty(input);
    }


    public static bool IsNullOrWhiteSpace(this string input)
    {
      return String.IsNullOrWhiteSpace(input);
    }

    public static string ToCamelCase(this string input)
    {
      if (!String.IsNullOrEmpty(input) && input.Length > 1)
      {
        return Char.ToLowerInvariant(input[0]) + input.Substring(1);
      }
      return input;
    }

    public static string ToPascalCase(this string input)
    {
      if (!String.IsNullOrEmpty(input) && input.Length > 1)
      {
        return Char.ToUpperInvariant(input[0]) + input.Substring(1);
      }
      return input;
    }

    public static string ToCamelCaseId(this string input)
    {
      if (String.IsNullOrEmpty(input))
      {
        return input;
      }

      if (input.Length < 2)
      {
        return input.ToLowerInvariant();
      }

      string[] parts = input.Split('.');

      return String.Join(".", parts.Select(x => x.ToCamelCase()));
    }


    public static string ToPascalCaseId(this string input)
    {
      if (String.IsNullOrEmpty(input))
      {
        return input;
      }

      if (input.Length < 2)
      {
        return input.ToUpperInvariant();
      }

      string[] parts = input.Split('.');

      return String.Join(".", parts.Select(x => x.ToPascalCase()));
    }


    public static string RemoveNewLines(this string input)
    {
      if (String.IsNullOrEmpty(input))
      {
        return input;
      }

      return newLineCharsRegex.Replace(input, String.Empty).Trim();
    }
  }
}
