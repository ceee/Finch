namespace zero.Backoffice.UIComposition;

/// <summary>
/// Defines an area in the settings section
/// </summary>
public interface ISettingsArea
{
  /// <summary>
  /// Alias which used for the URL part
  /// </summary>
  string Alias { get; }

  /// <summary>
  /// Name of the settings area
  /// </summary>
  string Name { get; }

  /// <summary>
  /// Icon displayed next to the area name
  /// </summary>
  string Icon { get; }

  /// <summary>
  /// Further describe the area
  /// </summary>
  string Description { get; }

  /// <summary>
  /// Set a custom URL for this settings area link
  /// </summary>
  string CustomUrl { get; set; }
}