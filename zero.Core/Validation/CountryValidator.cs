using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Validation
{
  public class CountryValidator : ZeroValidator<ICountry, Country>
  {
    public CountryValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Code).Length(2).Unique(store);
      RuleFor(x => x.Name).Length(2, 120);
    }
  }
}
