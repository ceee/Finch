using FluentValidation;
using FluentValidation.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Validation;

public static class ServiceCollectionExtensions
{
  public static void AddGermanValidation(this IServiceCollection services)
  {
    ValidatorOptions.Global.LanguageManager.AddGermanOverrides();
    services.Replace<IdentityErrorDescriber, GermanIdentityErrorDescriber>(ServiceLifetime.Scoped);
  }


  static ILanguageManager AddGermanOverrides(this ILanguageManager manager)
  {
    if (manager is LanguageManager)
    {
      var lmanager = manager as LanguageManager;

      foreach (var kvp in FluentValidationGermanLanguage.Translations)
      {
        lmanager.AddTranslation("de", kvp.Key, kvp.Value);
      }
    }
		return manager;
	}
}