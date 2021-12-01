namespace zero.Api.Endpoints.Translations;

public class TranslationEdit : DisplayModel<Translation>
{
  public string Value { get; set; }

  public TranslationDisplay Display { get; set; }
}