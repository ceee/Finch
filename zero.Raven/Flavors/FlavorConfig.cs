using System.Text.Json.Serialization;

namespace zero.Raven;

/// <summary>
/// A flavor config holds information about a flavor implementation
/// </summary>
public class FlavorConfig
{
  /// <summary>
  /// Type of the associated entity
  /// </summary>
  [JsonIgnore]
  public Type FlavorType { get; private set; }

  /// <summary>
  /// Alias for querying
  /// </summary>
  public string Alias { get; set; }

  /// <summary>
  /// Name of the flavor
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Optional description
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// Icon of the flavor
  /// </summary>
  public string Icon { get; set; }


  [JsonIgnore]
  public Func<FlavorConfig, object> Construct { get; set; }


  public FlavorConfig(Type type)
  {
    FlavorType = type;
  }
}