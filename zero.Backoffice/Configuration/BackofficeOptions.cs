namespace zero.Backoffice.Configuration;

public class BackofficeOptions
{
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
  /// Configure search maps
  /// </summary>
  public SearchOptions Search { get; set; } = new();

  /// <summary>
  /// Options for configuring the vite development server
  /// </summary>
  public ZeroDevOptions DevServer { get; set; } = new();
}