using FluentValidation.Resources;

namespace zero.Localization;

public static class LanguageManagerExtensions
{
  public static ILanguageManager AddGermanOverrides(this ILanguageManager manager)
  {
    if (manager is LanguageManager languageManager)
    {
      foreach (var kvp in FluentValidationGermanLanguage.Translations)
      {
        languageManager.AddTranslation("de", kvp.Key, kvp.Value);
      }
    }
		return manager;
	}
}