namespace zero.Backoffice.UIComposition;

/// <summary>
/// Creates a new settings group in the backoffice application settings section
/// </summary>
public interface ISettingsGroup
{
  /// <summary>
  /// The name of the group (either a string or a translation key with @ prefix)
  /// </summary>
  string Name { get; set; }

  /// <summary>
  /// Areas/items within the group
  /// </summary>
  List<SettingsArea> Areas { get; set; }

  /// <summary>
  /// Add a new area to the group
  /// </summary>
  void Add(string alias, string name, string description = null, string icon = null, string customUrl = null);
}