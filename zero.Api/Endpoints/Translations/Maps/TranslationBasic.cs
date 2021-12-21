namespace zero.Api.Endpoints.Translations;

public class TranslationBasic : ZeroIdEntity
{
  public string Key { get; set; }

  public DateTimeOffset CreatedDate { get; set; }

  public string Flavor { get; set; }

  public string Value { get; set; }

  public TranslationDisplay Display { get; set; }
}