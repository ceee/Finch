using System;
using System.Globalization;

namespace zero.Core.Extensions
{
  public static class StringExtensions
  {
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
  }
}
