namespace zero.Backoffice.Configuration;

public class BackofficeOptions
{
  /// <summary>
  /// Hash for all zero assets
  /// </summary>
  public string AssetHash { get; set; }

  /// <summary>
  /// Public path to zero assets
  /// </summary>
  public string AssetPath { get; set; }

  /// <summary>
  /// Paths in the backoffice which are not handled by zero
  /// </summary>
  public List<string> ExcludedPaths { get; private set; } = new();

  /// <summary>
  /// Define icon sets which can be used in icon pickers (and also in backoffice rendering)
  /// </summary>
  public List<BackofficeIconSet> IconSets { get; set; } = new();

  /// <summary>
  /// Authentication configuration for external services
  /// </summary>
  public ExternalServicesOptions ExternalServices { get; set; } = new();

  /// <summary>
  /// Options for configuring the vite development server
  /// </summary>
  public ZeroDevOptions DevServer { get; set; } = new();

  /// <summary>
  /// Default language ISO code
  /// </summary>
  public string DefaultLanguage { get; set; }

  /// <summary>
  /// Language ISO codes which are supported by the zero backoffice
  /// </summary>
  public string[] SupportedLanguages { get; internal set; }
}