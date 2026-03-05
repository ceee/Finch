using FluentValidation;
using zero.Validation.Validators;

namespace zero.Validation;

public static partial class ValidatorExtensions
{
  /// <summary>
  /// Validate a color input as HEX (#aabbccdd or #aabbcc or #abc)
  /// </summary>
  public static IRuleBuilderOptions<T, string> Hex<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.SetValidator(new HexValidator<T>());
  }

  /// <summary>
  /// Validate an email
  /// </summary>
  public static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.SetValidator(new UrlValidator<T>());
  }

  /// <summary>
  /// Validate an email
  /// </summary>
  public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.SetValidator(new EmailValidator<T>());
  }

  /// <summary>
  /// Validate one or multiple emails
  /// </summary>
  public static IRuleBuilderOptions<T, string> Emails<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.SetValidator(new EmailsValidator<T>());
  }

  /// <summary>
  /// Validates a culture identifier
  /// </summary>
  public static IRuleBuilderOptions<T, string> Culture<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.SetValidator(new CultureValidator<T>());
  }
}
