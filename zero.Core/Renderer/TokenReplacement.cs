using System.Collections.Generic;
using zero.Core.Extensions;

namespace zero.Core.Renderer
{
  public class TokenReplacement
  {
    const char BEGIN = '{';

    const char END = '}';

    public static string Apply(string text, Dictionary<string, string> tokens)
    {
      if (text.IsNullOrWhiteSpace())
      {
        return text;
      }

      foreach ((string key, string value) in tokens)
      {
        string tokenKey = key.EnsureStartsWith(BEGIN).EnsureEndsWith(END);
        string tokenValue = value; // TODO escape for HTML?

        text = text.Replace(tokenKey, value);
      }

      return text;
    }
  }
}
