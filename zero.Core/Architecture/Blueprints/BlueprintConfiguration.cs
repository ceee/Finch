namespace zero.Architecture;

/// <summary>
/// Defines a base entity which is synced and properties which are overridden
/// </summary>
public class BlueprintConfiguration
{
  /// <summary>
  /// Id of the entity the synchronisation is based upon
  /// </summary>
  public string Id { get; set; }

  /// <summary>
  /// A shallow copy of a blueprint can not be changed and is always fully synchronised with the parent entity
  /// </summary>
  public bool IsShallow { get; set; }

  /// <summary>
  /// Properties which are not synced and have their own values
  /// </summary>
  public string[] Desync { get; set; } = Array.Empty<string>();

  /// <summary>
  /// Additional custom sync options
  /// </summary>
  public Dictionary<string, string> Options = new();
}