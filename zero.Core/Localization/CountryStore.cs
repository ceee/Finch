using FluentValidation.Results;

namespace zero.Localization;

public class CountryStore : EntityStore<Country>, ICountryStore
{
  public CountryStore(IStoreContext context) : base(context) { }


  public override Task<ValidationResult> Validate(ZeroValidationContext ctx, Country model)
  {
    return new CountryValidator(ctx).ValidateAsync(model);
  }
}


public interface ICountryStore : IEntityStore<Country> { }