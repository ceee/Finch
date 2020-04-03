using System;
using System.Text;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class Alias
  {
    private const char HYPHEN = '-';

    private const char PLUS = '+';

    private const char AMPERSAND = '&';


    /// <summary>
    /// Converts a term to a safe alias (suitable for URLs)
    /// </summary>
    public static string Generate(string value)
    {
      if (String.IsNullOrWhiteSpace(value))
      {
        return String.Empty;
      }

      char previous = default;
      StringBuilder output = new StringBuilder();

      for (int i = 0; i < value.Length; i++)
      {
        // get character in lower case
        char character = char.ToLower(value[i]);
        char target;

        // do not handle surrogates
        if (char.IsSurrogate(character))
        {
          continue;
        }

        // special replacements accents + umlauts
        if (character.TryReplaceAccent(out char[] replacement))
        {
          if (replacement.Length > 1)
          {
            output.Append(replacement);
            output.Remove(output.Length - 1, 1);
          }
          target = replacement[replacement.Length - 1];
        }
        // append character a-z, 0-9
        else if (character.IsAZor09())
        {
          target = character;
        }
        // + sign for + and &
        else if (character == PLUS || character == AMPERSAND)
        {
          target = PLUS;
        }
        // add hyphen for all other characters
        else
        {
          target = HYPHEN;
        }

        // add default characters
        if (target != HYPHEN && target != PLUS)
        {
          output.Append(target);
        }
        // add hyphen if it isn't first and previous char is not + or -
        else if (target == HYPHEN && previous != default && previous != PLUS && previous != HYPHEN)
        {
          output.Append(target);
        }
        // add plus. do remove hyphen it is the previous character
        else if (target == PLUS)
        {
          if (previous == HYPHEN)
          {
            output.Remove(output.Length - 1, 1);
          }
          output.Append(target);
        }

        if (output.Length > 0)
        {
          previous = output[output.Length - 1];
        }
      }

      if (output.Length > 0 && !output[output.Length - 1].IsAZor09())
      {
        output.Remove(output.Length - 1, 1);
      }

      if (output.Length == 0)
      {
        output.Append(HYPHEN);
      }

      return output.ToString();
    }
  }
}