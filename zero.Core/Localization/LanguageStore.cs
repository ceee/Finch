using FluentValidation;

namespace zero.Localization;

public class LanguageStore : EntityStore<Language>, ILanguageStore
{
  public LanguageStore(IStoreContext context) : base(context) { }

  /// <inheritdoc />
  protected override void ValidationRules(ZeroValidator<Language> validator)
  {
    validator.RuleFor(x => x.Name).Length(2, 60);
    validator.RuleFor(x => x.Code).Length(2, 10).Culture();
    validator.RuleFor(x => x.IsDefault).Unique(Context.Store).When(x => x.IsDefault).WithMessage("@language.errors.default_unique");
    validator.RuleFor(x => x.IsDefault).ExpectAnyUnique(Context.Store, expectedValue: true).When(x => !x.IsDefault).WithMessage("@language.errors.needs_default");
    validator.RuleFor(x => x.InheritedLanguageId).Must((entity, value) => !entity.Id.Equals(value, StringComparison.InvariantCultureIgnoreCase)).When(x => !x.Id.IsNullOrEmpty()).WithMessage("@language.errors.fallback_invalid");
    validator.RuleFor(x => x.InheritedLanguageId).Equal((string)null).When(x => x.IsDefault).WithMessage("@language.errors.default_no_fallback");
    validator.RuleFor(x => x.InheritedLanguageId).Exists(Context.Store);
    validator.RuleFor(x => x.IsOptional).Equal(false).When(x => x.IsDefault).WithMessage("@language.errors.default_not_optional");
  }
}


public interface ILanguageStore : IEntityStore<Language> { }