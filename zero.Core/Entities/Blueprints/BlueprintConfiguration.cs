using System.Collections.Generic;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
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
    /// Properties which are not synced and have their own values
    /// </summary>
    public string[] Desync { get; set; } = new string[] { };

    /// <summary>
    /// Additional custom sync options
    /// </summary>
    public Dictionary<string, string> Options = new Dictionary<string, string>();
  }
}
