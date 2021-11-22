namespace zero.Backoffice.Configuration;

/// <summary>
/// Define a backoffice icon set
/// </summary>
public class BackofficeIconSet
{
  /// <summary>
  /// The alias for reference
  /// </summary>
  public string Alias { get; set; }

  /// <summary>
  /// Name of the icon set
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Optional symbol identifier prefix (by default the alias is used)
  /// </summary>
  public string Prefix { get; set; }

  /// <summary>
  /// Path to the SVG sprite containing addressable symbols
  /// </summary>
  public string SpritePath { get; set; }
}