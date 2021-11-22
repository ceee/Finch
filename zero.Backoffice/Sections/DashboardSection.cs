namespace zero.Backoffice.Sections;

/// <summary>
/// The dashboard aggregates data from all sections
/// </summary>
public class DashboardSection : IInternalSection
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