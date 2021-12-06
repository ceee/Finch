namespace zero.Applications;

public class ApplicationRegistration
{
  /// <summary>
  /// Alias for this tenant
  /// </summary>
  public string Key { get; set; }

  /// <summary>
  /// Name of the tenant
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Raven database name for application data
  /// </summary>
  public string Database { get; set; }

  /// <summary>
  /// Generic contact email. Can be used in various locations
  /// </summary>
  public string Email { get; set; }

  /// <summary>
  /// All assigned domains for this application
  /// </summary>
  public Uri[] Domains { get; set; } = Array.Empty<Uri>();

  /// <summary>
  /// Features which are enabled for this application.
  /// Can be user-defined and affect both backoffice and frontend
  /// </summary>
  public List<string> Features { get; set; } = new();
}