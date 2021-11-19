namespace zero.Core.Entities
{
  /// <summary>
  /// Define a backoffice icon set
  /// </summary>
  public class IconSet
  {
    /// <summary>
    /// The alias
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Name of the icon set
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Prefix for addressing symbols (by default the alias)
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// Path to the SVG sprite
    /// </summary>
    public string SpritePath { get; set; }
  }
}
