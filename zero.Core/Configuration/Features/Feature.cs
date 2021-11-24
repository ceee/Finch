namespace zero.Configuration;

/// <summary>
/// A feature can affect both the backoffice and the frontend
/// </summary>
public class Feature
{
  /// <summary>
  /// The alias
  /// </summary>
  public string Alias { get; set; }

  /// <summary>
  /// The name of the feature
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Additional description
  /// </summary>
  public string Description { get; set; }
}