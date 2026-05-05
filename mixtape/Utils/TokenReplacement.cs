using System.Text.RegularExpressions;

namespace Mixtape.Utils;

public class TokenReplacement
{
  static Regex TokenRegex;


  static TokenReplacement()
  {
    TokenRegex = new Regex("{([\\w-_.]+)}", RegexOptions.IgnoreCase);
  }


  public static string Apply(string text, Dictionary<string, string> tokens)
  {
    if (text.IsNullOrWhiteSpace())
    {
      return text;
    }

    MatchCollection matches = TokenRegex.Matches(text);

    foreach (Match match in matches)
    {
      if (!match.Success)
      {
        continue;
      }

      string original = match.Value;
      string token = match.Groups[1].Value;

      if (tokens.ContainsKey(token))
      {
        text = text.Replace(original, tokens[token]);
      }
    }

    return text;

    //foreach ((string key, string value) in tokens)
    //{
    //  string tokenKey = key.EnsureStartsWith(BEGIN).EnsureEndsWith(END);
    //  string tokenValue = value; // TODO escape for HTML?

    //  text = text.Replace(tokenKey, value);
    //}

    //return text;
  }
}
