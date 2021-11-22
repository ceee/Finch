namespace zero.Backoffice.Sections;

/// <summary>
/// Website and backoffice settings
/// </summary>
public class SettingsSection : IInternalSection
{
  /// <inheritdoc />
  public string Alias => Constants.Sections.Settings;

  /// <inheritdoc />
  public string Name => "@sections.item.settings";

  /// <inheritdoc />
  public string Icon => "fth-settings";

  /// <inheritdoc />
  public string Color => null;

  /// <inheritdoc />
  public IList<IChildSection> Children => new List<IChildSection>();
}
