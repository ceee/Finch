using System.Collections.Generic;
using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Sections
{
  /// <summary>
  /// Global list entities
  /// </summary>
  public class ListsSection : ISection, IBuiltInSection
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Lists;

    /// <inheritdoc />
    public string Name => "@sections.item.lists";

    /// <inheritdoc />
    public string Icon => "fth-layers";

    /// <inheritdoc />
    public string Color => "#f9c202";

    /// <inheritdoc />
    public IList<IChildSection> Children => new List<IChildSection>();
  }
}
