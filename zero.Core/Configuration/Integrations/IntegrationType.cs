using FluentValidation;
using System.Text.Json.Serialization;

namespace zero.Configuration;

/// <summary>
/// An integration is an application part which has a public configuration per app.
/// It's up to the user to provide functionality.
/// </summary>
public class IntegrationType
{
  /// <summary>
  /// Type of the associated entity
  /// </summary>
  [JsonIgnore]
  public Type ModelType { get; private set; }

  /// <summary>
  /// Alias to find the editor schema
  /// </summary>
  public string EditorAlias { get; set; }

  /// <summary>
  /// Alias for querying
  /// </summary>
  public string Alias { get; set; }

  /// <summary>
  /// Name of the flavor
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Group integrations by tags
  /// </summary>
  public List<string> Tags { get; set; } = new();

  /// <summary>
  /// Optional description
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// Image of the integration
  /// </summary>
  public string ImagePath { get; set; }

  /// <summary>
  /// Set a validator for this integration
  /// </summary>
  [JsonIgnore]
  public IValidator Validator { get; set; }

  [JsonIgnore]
  public Func<FlavorConfig, object> Construct { get; set; }

  public IntegrationType(Type type)
  {
    ModelType = type;
  }
}
