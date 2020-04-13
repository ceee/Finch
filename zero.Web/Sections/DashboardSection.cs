using System.Collections.Generic;
using zero.Core;
using zero.Core.Entities;

namespace zero.Web.Sections
{
  /// <summary>
  /// The dashboard aggregates data from all sections
  /// </summary>
  public class DashboardSection : ISection, IBuiltInSection
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Dashboard;

    /// <inheritdoc />
    public string Name => "@sections.item.dashboard";

    /// <inheritdoc />
    public string Icon => "fth-pie-chart";

    /// <inheritdoc />
    public string Color => null;

    /// <inheritdoc />
    public IList<IChildSection> Children => new List<IChildSection>();
  }
}
