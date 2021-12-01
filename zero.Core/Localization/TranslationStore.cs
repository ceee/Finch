using FluentValidation;

namespace zero.Localization;

public class TranslationStore : EntityStore<Translation>, ITranslationStore
{
  public TranslationStore(IStoreContext context) : base(context) { }


  /// <inheritdoc />
  public async Task<string> LoadString(string id)
  {
    return (await Load(id))?.Value;
  }


  protected override void ValidationRules(ZeroValidator<Translation> validator)
  {
    validator.RuleFor(x => x.Key).NotEmpty().Length(2, 300).Unique(Context.Store);
    validator.RuleFor(x => x.Value).MaximumLength(10 * 1000);
  }
}


public interface ITranslationStore : IEntityStore<Translation>
{
  /// <summary>
  /// Get a translated string by id
  /// </summary>
  Task<string> LoadString(string id);
}