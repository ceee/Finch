using System.Collections.Generic;
using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Sections
{
  /// <summary>
  /// Global list entities
  /// </summary>
  public class ListsSection : ISection
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Lists;

    /// <inheritdoc />
    public string Name => "@ui_sections_lists";

    /// <inheritdoc />
    public string Icon => "fth-layers";

    /// <inheritdoc />
    public IList<IChildSection> Children => null;
  }
}
