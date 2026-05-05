using FluentValidation;

namespace Mixtape.Numbers;

public class NumberValidator : MixtapeValidator<Number>
{
  public NumberValidator()
  {
    RuleFor(x => x.Template).NotEmpty().Length(2, 60);
    RuleFor(x => x.Template).Must(x => x != null && x.Contains("{number}"));
    RuleFor(x => x.StartNumber).GreaterThanOrEqualTo(0);
    RuleFor(x => x.MinLength).ExclusiveBetween(1, 32);
    RuleFor(x => x.Step).GreaterThanOrEqualTo(1);
  }
}