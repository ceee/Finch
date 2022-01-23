using FluentValidation.Results;

namespace zero.Localization;

public class TranslationStore : EntityStore<Translation>, ITranslationStore
{
  public TranslationStore(IStoreContext context) : base(context) { }


  /// <inheritdoc />
  public async Task<string> LoadString(string id)
  {
    return (await Load(id))?.Value;
  }


  public override Task<ValidationResult> Validate(ZeroValidationContext ctx, Translation model)
  {
    return new TranslationValidator(ctx).ValidateAsync(model);
  }
}


public interface ITranslationStore : IEntityStore<Translation>
{
  /// <summary>
  /// Get a translated string by id
  /// </summary>
  Task<string> LoadString(string id);
}