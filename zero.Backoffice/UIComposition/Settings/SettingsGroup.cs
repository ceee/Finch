namespace zero.Backoffice.UIComposition;

/// <inheritdoc />
public class SettingsGroup : ISettingsGroup
{
  /// <inheritdoc />
  public string Name { get; set; }

  /// <inheritdoc />
  public List<SettingsArea> Areas { get; set; } = new();


  public SettingsGroup(string name)
  {
    Name = name;
  }

  /// <inheritdoc />
  public void Add(string alias, string name, string description = null, string icon = null, string customUrl = null)
  {
    Areas.Add(new SettingsArea(alias, name, description, icon, customUrl));
  }
}