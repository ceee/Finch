using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Mixtape.Validation.Validators;

public abstract class RegexValidator<T> : PropertyValidator<T, string>
{
  private readonly Regex _regex = null;

  public RegexValidator(Regex regex)
  {
    _regex = regex; 
  }

  protected static Regex CreateRegex(string expression)
  {
    const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
    return new Regex(expression, options, TimeSpan.FromSeconds(2.0));
  }

  protected override string GetDefaultMessageTemplate(string errorCode) => Localized(errorCode, Name);

  public override bool IsValid(FluentValidation.ValidationContext<T> context, string value)
  {
    if (value == null) return true;

    if (!_regex.IsMatch(value))
    {
      return false;
    }

    return true;
  }
}