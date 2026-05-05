using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq.Expressions;

namespace Mixtape.Localization;

public class CultureResolver : ICultureResolver
{
  /// <inheritdoc />
  public CultureInfo Current { get; protected set; }

  protected ILogger<CultureResolver> Logger { get; }

  protected IMessageAggregator MessageAggregator { get; }


  public CultureResolver(ILogger<CultureResolver> logger, IMessageAggregator messageAggregator)
  {
    Logger = logger;
    MessageAggregator = messageAggregator;
  }


  /// <inheritdoc />
  public Task<CultureInfo> Resolve(IMixtapeContext context)
  {
    if (!TryConvert(context.Options.Language, out CultureInfo culture))
    {
      culture = CultureInfo.CurrentCulture;
    }

    Set(culture);

    return Task.FromResult(culture);
  }


  /// <inheritdoc />
  public bool TryConvert(string isoCode, out CultureInfo culture)
  {
    try
    {
      culture = CultureInfo.CreateSpecificCulture(isoCode.Replace('_', '-'));

      if (culture.ThreeLetterISOLanguageName.IsNullOrEmpty())
      {
        throw new Exception("ThreeLetterISOLanguageName is empty");
      }
      if (culture.ThreeLetterISOLanguageName == "ivl")
      {
        throw new Exception("Invariant language is not allowed");
      }

      return true;
    }
    catch (Exception ex)
    {
      Logger.LogWarning(ex, "Could not create culture from Language code {code}", isoCode);
      culture = null;
      return false;
    }
  }


  /// <inheritdoc />
  public void Set(CultureInfo culture)
  {
    CultureInfo.CurrentCulture = culture;
    CultureInfo.CurrentUICulture = culture;
    ValidatorOptions.Global.LanguageManager.Culture = culture;
    Current = culture;
    MessageAggregator.Publish(new CultureChangeMessage()
    {
      Culture = culture
    });
  }


  /// <inheritdoc />
  public void Subscribe(Expression<Func<CultureChangeMessage, Task>> handle)
  {
    MessageAggregator.Subscribe(handle);
  }
}


public interface ICultureResolver
{
  /// <summary>
  /// Current culture
  /// </summary>
  CultureInfo Current { get; }

  /// <summary>
  /// Resolves the current application from either the backoffice user (in case it is backoffice request)
  /// or the domain (in case it is frontend request).
  /// </summary>
  Task<CultureInfo> Resolve(IMixtapeContext context);

  /// <summary>
  /// Tries to convert an ISO code to a culture
  /// </summary>
  bool TryConvert(string isoCode, out CultureInfo culture);

  /// <summary>
  /// Set a new culture for this request
  /// </summary>
  void Set(CultureInfo culture);

  /// <summary>
  /// Subscribe to culture change
  /// </summary>
  void Subscribe(Expression<Func<CultureChangeMessage, Task>> handle);
}
