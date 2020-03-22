using System.Collections.Generic;
using unjo.Core;
using unjo.Core.Entities.Sections;

namespace unjo.Web.Sections
{
  /// <summary>
  /// Website and backoffice settings
  /// </summary>
  public class SettingsSection : ISection
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Settings;

    /// <inheritdoc />
    public string Name => "@ui_sections_settings";

    /// <inheritdoc />
    public string Icon => "fth-settings";

    /// <inheritdoc />
    public IList<IChildSection> Children => null;
  }
}
