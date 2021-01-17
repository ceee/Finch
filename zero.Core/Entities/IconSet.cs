namespace zero.Core.Entities
{
  /// <inheritdoc />
  public class IconSet : IIconSet
  {
    /// <inheritdoc />
    public string Alias { get; set; }

    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public string Prefix { get; set; }

    /// <inheritdoc />
    public string SpritePath { get; set; }
  }

  /// <summary>
  /// Define a backoffice icon set
  /// </summary>
  public interface IIconSet
  {
    /// <summary>
    /// The alias
    /// </summary>
    string Alias { get; }

    /// <summary>
    /// Name of the icon set
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Prefix for addressing symbols (by default the alias)
    /// </summary>
    string Prefix { get; }

    /// <summary>
    /// Path to the SVG sprite
    /// </summary>
    string SpritePath { get; }
  }
}
