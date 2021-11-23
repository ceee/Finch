namespace zero.Backoffice.Models;

/// <summary>
/// The modifier displays a small icon (with hover text) next to the main item icon
/// </summary>
public class TreeItemModifier
{
  /// <summary>
  /// Name of the modifier
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Icon to display
  /// </summary>
  public string Icon { get; set; }

  public TreeItemModifier() { }

  public TreeItemModifier(string name, string icon)
  {
    Name = name;
    Icon = icon;
  }
}
