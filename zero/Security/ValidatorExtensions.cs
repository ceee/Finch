using FluentValidation;
using zero.Security;

namespace zero.Validation;

public static partial class ValidatorExtensions
{
  /// <summary>
  /// Validates a captcha token
  /// </summary>
  public static IRuleBuilderOptions<T, string> Captcha<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
    return ruleBuilder.SetAsyncValidator(new CaptchaValidator<T>());
  }
}
