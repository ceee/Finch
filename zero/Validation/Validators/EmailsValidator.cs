using FluentValidation.Validators;

namespace zero.Validation.Validators;

public class EmailsValidator<T> : PropertyValidator<T, string>, IEmailValidator
{
  public override string Name => "EmailValidator";

  protected override string GetDefaultMessageTemplate(string errorCode) => Localized(errorCode, Name);

  public override bool IsValid(FluentValidation.ValidationContext<T> context, string value)
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
      if (!EmailValidator<T>.ValidateEmail(mail))
      {
        return false;
      }
    }

    return true;
  }
}