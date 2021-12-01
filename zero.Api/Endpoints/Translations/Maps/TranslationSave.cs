namespace zero.Api.Endpoints.Translations;

public class TranslationSave : SaveModel<Translation>
{
  public string Value { get; set; }

  public TranslationDisplay Display { get; set; }
}