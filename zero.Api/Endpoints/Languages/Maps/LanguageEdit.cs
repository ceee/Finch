namespace zero.Api.Endpoints.Languages;

public class LanguageEdit : DisplayModel<Language>
{
  public string Code { get; set; }

  public bool IsDefault { get; set; }

  public string InheritedLanguageId { get; set; }
}