using System.Collections.Generic;
using unjo.Core;
using unjo.Core.Entities.Sections;

namespace unjo.Web.Sections
{
  /// <summary>
  /// Manage the page tree in this section
  /// </summary>
  public class PagesSection : ISection
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Pages;

    /// <inheritdoc />
    public string Name => "@ui_sections_pages";

    /// <inheritdoc />
    public string Icon => "fth-folder";

    /// <inheritdoc />
    public IList<IChildSection> Children => null;
  }
}
