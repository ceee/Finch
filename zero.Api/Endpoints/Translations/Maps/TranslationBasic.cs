namespace zero.Api.Endpoints.Translations;

public class TranslationBasic : BasicModel<Translation>
{
  public string Value { get; set; }

  public TranslationDisplay Display { get; set; }
}