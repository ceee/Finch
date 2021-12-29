namespace zero.Backoffice.UIComposition;

/// <summary>
/// The dashboard aggregates data from all sections
/// </summary>
public class DashboardSection : IInternalBackofficeSection
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
  public int Sort => 0;

  /// <inheritdoc />
  public IList<IChildBackofficeSection> Children => new List<IChildBackofficeSection>();
}