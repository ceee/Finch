namespace zero.Stores;

public class StoreConfig
{
  /// <summary>
  /// Whether to include entities with IsActive=false in queries. Defaults to false.
  /// </summary>
  public bool IncludeInactive { get; set; } = false;

  /// <summary>
  /// Override the database which is resolved automatically by default (based on IZeroContext and IApplicationResolver).
  /// </summary>
  public string Database { get; set; }
}