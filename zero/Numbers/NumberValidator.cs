using FluentValidation;

namespace zero.Numbers;

public class NumberValidator : ZeroValidator<Number>
{
  public NumberValidator()
  {
    //if (ctx.Context == null)
    //{
    //  throw new ArgumentNullException("ctx.Context", "NumberValidator requires access to ");
    //}

    RuleFor(x => x.Template).NotEmpty().Length(2, 60);
    RuleFor(x => x.Template).Must(x => x != null && x.Contains("{number}")).WithMessage("@shop.number.errors.invalid_template");
    RuleFor(x => x.StartNumber).GreaterThanOrEqualTo(0);
    RuleFor(x => x.MinLength).ExclusiveBetween(1, 32);
  }
}