namespace zero.Applications;

/// <summary>
/// An application is a website or app. zero can host multiple websites at once which share common assets
/// </summary>
[RavenCollection("Applications")]
public class Application : ZeroEntity
{
  /// <summary>
  /// Raven database name for application data
  /// </summary>
  public string Database { get; set; }

  /// <summary>
  /// Full company or product name
  /// </summary>
  public string FullName { get; set; }

  /// <summary>
  /// Generic contact email. Can be used in various locations
  /// </summary>
  public string Email { get; set; }

  /// <summary>
  /// Image of the application
  /// </summary>
  public string ImageId { get; set; }

  /// <summary>
  /// Simple image of the application (can be used as favicon)
  /// </summary>
  public string IconId { get; set; }

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