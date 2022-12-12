using FluentValidation;
using System.Globalization;
using System.Net.Mail;

namespace zero.Validation;

public static class ValidatorExtensions
{
  private const char DOT = '.';

  private const char KLAMMERAFFE = '@';

  private static string HEX_REGEX = "(^$)|(#[0-9a-fA-F]{3,8})";

  /// <summary>
  /// Validate a color input as HEX (#aabbccdd or #aabbcc or #abc)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Hex<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Matches(HEX_REGEX).WithMessage("@errors.forms.hex_format");
  }


  /// <summary>
  /// Validate an email
  /// </summary>
  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must((root, value, context) =>
    {
      return value.IsNullOrWhiteSpace() || Uri.IsWellFormedUriString(value, UriKind.Absolute);
    }).WithMessage("@errors.forms.url_format");
  }


  /// <summary>
  /// Validate an email
  /// </summary>
  public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must((root, value, context) => ValidateEmail(value)).WithMessage("@errors.forms.email_invalid");
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
        if (!ValidateEmail(mail))
        {
          return false;
        }
      }

      return true;
    }).WithMessage("@errors.forms.emails_invalid");
  }


  private static bool ValidateEmail(string value)
  {
    if (value == null)
    {
      return false;
    }

    int index = value.IndexOf(KLAMMERAFFE);

    return
      index > 0 &&
      index != value.Length - 1 &&
      index == value.LastIndexOf(KLAMMERAFFE) &&
      MailAddress.TryCreate(value, out _);
  }


  /// <summary>
  /// Validates a culture identifier
  /// </summary>
  public static IRuleBuilderOptions<T, string> Culture<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.Must((root, value, context) =>
    {
      if (value.IsNullOrWhiteSpace())
      {
        return true;
      }

      try
      {
        CultureInfo info = CultureInfo.GetCultureInfo(value);
        return info != null && !info.EnglishName.Equals(value, StringComparison.InvariantCultureIgnoreCase);
      }
      catch
      {
        return false;
      }
    }).WithMessage("@errors.forms.culture");
  }
}
