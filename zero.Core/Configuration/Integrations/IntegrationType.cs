using FluentValidation;
using System.Text.Json.Serialization;

namespace zero.Configuration;

/// <summary>
/// An integration is an application part which has a public configuration per app.
/// It's up to the user to provide functionality.
/// </summary>
public class IntegrationType : FlavorConfig
{
  public IntegrationType(Type type) : base(type) { }

  /// <summary>
  /// Alias to find the editor schema
  /// </summary>
  public string EditorAlias { get; set; }

  /// <summary>
  /// Group integrations by tags
  /// </summary>
  public List<string> Tags { get; set; } = new();

  /// <summary>
  /// Image of the integration
  /// </summary>
  public string ImagePath { get; set; }

  /// <summary>
  /// Set a validator for this integration
  /// </summary>
  [JsonIgnore]
  public IValidator Validator { get; set; }
}
