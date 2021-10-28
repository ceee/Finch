using FluentValidation;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class CountryValidator : ZeroValidator<Country>
  {
    public CountryValidator(IZeroStore store)
    {
      RuleFor(x => x.Code).Length(2).Unique(store);
      RuleFor(x => x.Name).Length(2, 120);
    }
  }
}
