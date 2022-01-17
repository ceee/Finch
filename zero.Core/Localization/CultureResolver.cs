using FluentValidation;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using System.Globalization;

namespace zero.Localization;

public class CultureResolver : ICultureResolver
{
  protected ILogger<CultureResolver> Logger { get; private set; }


  public CultureResolver(ILogger<CultureResolver> logger)
  {
    Logger = logger;
  }


  /// <inheritdoc />
  public async Task<CultureInfo> Resolve(IZeroContext context)
  {
    // TODO this is just fake, we need to correctly resolve culture here

    if (context.IsBackofficeRequest)
    {

    }
    else
    {
      //var session = context.Store.Session();
      //Language language = await session.Query<Language>().FirstOrDefaultAsync();
      Language language = new() { Code = "de-DE" };

      if (language == null)
      {
        Logger.LogWarning("Could not set request culture as there is no available Language stored");
        return CultureInfo.CurrentCulture;
      }

      try
      {
        CultureInfo culture = CultureInfo.CreateSpecificCulture(language.Code);

        if (culture.ThreeLetterISOLanguageName.IsNullOrEmpty())
        {
          throw new Exception("ThreeLetterISOLanguageName is empty");
        }

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        ValidatorOptions.Global.LanguageManager.Culture = culture;
      }
      catch (Exception ex)
      {
        Logger.LogError(ex, "Could not create culture from Language code {code}", language.Code);
        return CultureInfo.CurrentCulture;
      }
    }

    return CultureInfo.CurrentCulture;
  }
}


public interface ICultureResolver
{
  /// <summary>
  /// Resolves the current application from either the backoffice user (in case it is backoffice request)
  /// or the domain (in case it is frontend request).
  /// </summary>
  Task<CultureInfo> Resolve(IZeroContext context);
}
