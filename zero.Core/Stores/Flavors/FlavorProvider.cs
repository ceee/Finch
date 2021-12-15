using System.Text.Json.Serialization;

namespace zero.Stores;

/// <summary>
/// A flavor provider is attached to an entity (which has ISupportsFlavors) and contains all flavors
/// </summary>
public class FlavorProvider
{
  public bool CanUseWithoutFlavors { get; set; } = true;

  public string DefaultFlavor { get; set; }

  [JsonIgnore]
  public Type BaseType { get; set; }

  public List<FlavorConfig> Flavors { get; set; } = new();
}