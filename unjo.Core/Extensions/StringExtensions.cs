using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace unjo.Core.Extensions
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
  }
}
