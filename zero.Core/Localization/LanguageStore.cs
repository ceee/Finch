using FluentValidation.Results;

namespace zero.Localization;

public class LanguageStore : EntityStore<Language>, ILanguageStore
{
  public LanguageStore(IStoreContext context) : base(context) { }


  public override Task<ValidationResult> Validate(ZeroValidationContext ctx, Language model)
  {
    return new LanguageValidator(ctx).ValidateAsync(model);
  }
}


public interface ILanguageStore : IEntityStore<Language> { }