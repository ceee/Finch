namespace zero.Backoffice.Sections;

/// <summary>
/// A section is a main part of the backoffice application
/// </summary>
public class BackofficeSection : IBackofficeSection, IChildBackofficeSection
{
  /// <inheritdoc />
  public string Alias { get; set; }

  /// <inheritdoc />
  public string Name { get; set; }

  /// <inheritdoc />
  public string Icon { get; set; }

  /// <summary>
  /// HEX color (#aabbcc or #abc). Defaults to a neutral color
  /// </summary>
  public string Color { get; set; }

  /// <inheritdoc />
  public IList<IChildBackofficeSection> Children { get; } = new List<IChildBackofficeSection>();


  public BackofficeSection() { }

  public BackofficeSection(string alias, string name, string icon = null, string color = null)
  {
    Alias = alias;
    Name = name;
    Icon = icon;
    Color = color;
  }
}
