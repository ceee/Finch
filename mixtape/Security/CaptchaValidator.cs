using FluentValidation;
using FluentValidation.Validators;
using PowCapServer.Abstractions;

namespace Mixtape.Security;

public sealed class CaptchaValidator<T> : AsyncPropertyValidator<T, string>
{
  public override string Name => "CaptchaValidator";

  protected override string GetDefaultMessageTemplate(string errorCode) => Localized(errorCode, Name);

  public override async Task<bool> IsValidAsync(ValidationContext<T> context, string value, CancellationToken cancellation)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return false;
    }

    ICaptchaService captchaService = StaticCaptchaService.Get();

    if (captchaService is null)
    {
      return false;
    }

    return await captchaService.ValidateCaptchaTokenAsync(value, cancellation);
  }
}