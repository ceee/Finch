using FluentValidation;
using System;
using System.Linq;

namespace zero.Core.Extensions
{
  public static class ValidatorExtensions
  {
    private const char DOT = '.';

    private const char KLAMMERAFFE = '@';

    private static string HEX_REGEX = "#[0-9a-fA-F]{3,8}";

    /// <summary>
    /// Validate a color input as HEX (#aabbccdd or #aabbcc or #abc)
    /// </summary>
    public static IRuleBuilderOptions<T, string> Hex<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
      return ruleBuilder.Matches(HEX_REGEX).WithMessage("@errors/format_hex");
    }


    /// <summary>
    /// Validate an email
    /// </summary>
    public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
      return ruleBuilder.Must((root, value, context) =>
      {
        return value.IsNullOrWhiteSpace() || Uri.IsWellFormedUriString(value, UriKind.Absolute);
      }).WithMessage("@errors/invalid_uri");
    }


    /// <summary>
    /// Validate an email
    /// </summary>
    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
      return ruleBuilder.Must((root, value, context) =>
      {
        if (value.IsNullOrWhiteSpace())
        {
          return true;
        }

        int index = value.IndexOf(KLAMMERAFFE);

        if (index < 0 || index == value.Length - 1 || index != value.LastIndexOf(KLAMMERAFFE))
        {
          return false;
        }

        return true;
      }).WithMessage("@errors/invalid_mail");
    }


    /// <summary>
    /// Validate one or multiple emails
    /// </summary>
    public static IRuleBuilderOptions<T, string> Emails<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
      return ruleBuilder.Must((root, value, context) =>
      {
        if (value.IsNullOrWhiteSpace())
        {
          return true;
        }

        string[] mails = value.Split(',', ';').Select(x => x.Trim()).Where(x => !x.IsNullOrWhiteSpace()).ToArray();

        if (!mails.Any())
        {
          return false;
        }

        foreach (string mail in mails)
        {
          int index = value.IndexOf(KLAMMERAFFE);

          if (index < 0 || index == value.Length - 1 || index != value.LastIndexOf(KLAMMERAFFE))
          {
            return false;
          }
        }

        return true;
      }).WithMessage("@errors/invalid_mails");
    }
  }
}
