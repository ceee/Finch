using FluentValidation.Validators;
using System.Net.Mail;

namespace zero.Validation.Validators;

public class EmailValidator<T> : PropertyValidator<T, string>, IEmailValidator
{
  public override string Name => "EmailValidator";

  private const char KLAMMERAFFE = '@';

  protected override string GetDefaultMessageTemplate(string errorCode) => Localized(errorCode, Name);

  public override bool IsValid(FluentValidation.ValidationContext<T> context, string value)
  {
    return ValidateEmail(value);
  }

  public static bool ValidateEmail(string value)
  {
    if (value.IsNullOrWhiteSpace())
    {
      return true;
    }

    int index = value.IndexOf(KLAMMERAFFE);

    return
      index > 0 &&
      index != value.Length - 1 &&
      index == value.LastIndexOf(KLAMMERAFFE) &&
      MailAddress.TryCreate(value, out _);
  }
}