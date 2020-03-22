using System.Collections.Generic;
using unjo.Core;
using unjo.Core.Entities.Sections;

namespace unjo.Web.Sections
{
  /// <summary>
  /// The dashboard aggregates data from all sections
  /// </summary>
  public class DashboardSection : ISection
  {
    /// <inheritdoc />
    public string Alias => Constants.Sections.Dashboard;

    /// <inheritdoc />
    public string Name => "@ui_sections_dashboard";

    /// <inheritdoc />
    public string Icon => "fth-pie-chart";

    /// <inheritdoc />
    public IList<IChildSection> Children => null;
  }
}
