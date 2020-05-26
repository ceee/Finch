using System.Collections.Generic;
using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Sections
{
  /// <summary>
  /// Global list entities
  /// </summary>
  public class SpacesSection : ISection, IZeroInternal
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Spaces;

    /// <inheritdoc />
    public string Name => "@sections.item.spaces";

    /// <inheritdoc />
    public string Icon => "fth-layers";

    /// <inheritdoc />
    public string Color => "#f9c202";

    /// <inheritdoc />
    public IList<IChildSection> Children => new List<IChildSection>();
  }
}
