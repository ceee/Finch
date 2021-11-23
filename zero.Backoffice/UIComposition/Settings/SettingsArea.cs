namespace zero.Backoffice.UIComposition;

/// <inheritdoc />
public class SettingsArea : ISettingsArea
{
  /// <inheritdoc />
  public string Alias { get; }

  /// <inheritdoc />
  public string Name { get; }

  /// <inheritdoc />
  public string Icon { get; }

  /// <inheritdoc />
  public string Description { get; }

  /// <inheritdoc />
  public string CustomUrl { get; set; }


  public SettingsArea(string alias, string name, string description = null, string icon = null, string customUrl = null)
  {
    Alias = alias;
    Name = name;
    Icon = icon;
    Description = description;
    CustomUrl = customUrl;
  }
}