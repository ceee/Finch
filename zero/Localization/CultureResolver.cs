using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Security.Claims;

namespace zero.Localization;

public class CultureResolver : ICultureResolver
{
  protected ILogger<CultureResolver> Logger { get; private set; }


  public CultureResolver(ILogger<CultureResolver> logger)
  {
    Logger = logger;
  }


  /// <inheritdoc />
  public Task<CultureInfo> Resolve(IZeroContext context)
  {
    //var session = context.Store.Session();
    //Language language = await session.Query<Language>().FirstOrDefaultAsync();
    string isoCode = "de-AT";

    try
    {
      CultureInfo culture = CultureInfo.CreateSpecificCulture(isoCode);

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
      Logger.LogError(ex, "Could not create culture from Language code {code}", isoCode);
      return Task.FromResult(CultureInfo.CurrentCulture);
    }
    
    return Task.FromResult(CultureInfo.CurrentCulture);
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
