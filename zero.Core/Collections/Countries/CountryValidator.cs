using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class CountryValidator : ZeroValidator<ICountry>
  {
    public CountryValidator(IBackofficeStore store)
    {
      RuleFor(x => x.Code).Length(2).Unique(store);
      RuleFor(x => x.Name).Length(2, 120);
    }
  }
}
