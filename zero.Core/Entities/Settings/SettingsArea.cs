namespace zero.Core.Entities
{
  /// <summary>
  /// Defines an area in the settings section
  /// </summary>
  public class SettingsArea
  {
    /// <summary>
    /// Alias which used for the URL part
    /// </summary>
    public string Alias { get; }

    /// <summary>
    /// Name of the settings area
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Icon displayed next to the area name
    /// </summary>
    public string Icon { get; }

    /// <summary>
    /// Further describe the area
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Set a custom URL for this settings area link
    /// </summary>
    public string CustomUrl { get; set; }


    public SettingsArea() { }

    public SettingsArea(string alias, string name, string description = null, string icon = null, string customUrl = null)
    {
      Alias = alias;
      Name = name;
      Icon = icon;
      Description = description;
      CustomUrl = customUrl;
    }
  }


  public class InternalSettingsArea : SettingsArea
  {
    public InternalSettingsArea() { }
    public InternalSettingsArea(string alias, string name, string description = null, string icon = null) : base(alias, name, description, icon) { }
  }
}
