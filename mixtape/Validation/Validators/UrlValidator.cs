using FluentValidation.Validators;

namespace Mixtape.Validation.Validators;

public class UrlValidator<T> : PropertyValidator<T, string>
{
  public override string Name => "UrlValidator";

  protected override string GetDefaultMessageTemplate(string errorCode) => Localized(errorCode, Name);

  public override bool IsValid(FluentValidation.ValidationContext<T> context, string value)
  {
    return value.IsNullOrWhiteSpace() || Uri.IsWellFormedUriString(value, UriKind.Absolute);
  }
}