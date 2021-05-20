using FluentValidation;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using System;
using System.Globalization;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Cultures
{
  public class CultureResolver : ICultureResolver
  {
    protected ILogger<CultureResolver> Logger { get; private set; }

    public CultureResolver(ILogger<CultureResolver> logger)
    {
      Logger = logger;
    }


    /// <inheritdoc />
    public CultureInfo DefaultCurrentCulture => CultureInfo.CurrentCulture;


    /// <inheritdoc />
    public async Task<CultureInfo> Resolve(IZeroDocumentSession session)
    {
      // TODO this is just fake, we need to correctly resolve culture here
      Language language = await session.Query<Language>().FirstOrDefaultAsync();

      if (language == null)
      {
        Logger.LogWarning("Could not set request culture as there is no available Language stored");
        return CultureInfo.CurrentCulture;
      }

      try
      {
        CultureInfo culture = CultureInfo.CreateSpecificCulture(language.Code);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        ValidatorOptions.Global.LanguageManager.Culture = culture;
      }
      catch (Exception ex)
      {
        Logger.LogError(ex, "Could not create culture from Language code {code}", language.Code);
        return CultureInfo.CurrentCulture;
      }

      return CultureInfo.CurrentCulture;
    }
  }


  public interface ICultureResolver
  {
    /// <summary>
    /// Default value for CultureInfo.CurrentCulture
    /// </summary>
    CultureInfo DefaultCurrentCulture { get; }

    /// <summary>
    /// Resolves the current application from either the backoffice user (in case it is backoffice request)
    /// or the domain (in case it is frontend request).
    /// </summary>
    Task<CultureInfo> Resolve(IZeroDocumentSession session);
  }
}
