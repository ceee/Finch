using FluentValidation.Validators;
using System.Globalization;

namespace Mixtape.Validation.Validators;

public class CultureValidator<T> : PropertyValidator<T, string>
{
  public override string Name => "CultureValidator";

  protected override string GetDefaultMessageTemplate(string errorCode) => Localized(errorCode, Name);

  public override bool IsValid(FluentValidation.ValidationContext<T> context, string value)
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
  }
}