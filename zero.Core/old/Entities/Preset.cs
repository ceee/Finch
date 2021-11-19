using System;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public abstract class Preset
  {
    /// <summary>
    /// Key is used to query a preset
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Name of the preset
    /// </summary>
    public string Name { get; set; }
  }


  [Collection("Presets")]
  public class PresetOverride<T> : ZeroEntity where T : Preset
  {
    /// <summary>
    /// Type of the target model (used to query)
    /// </summary>
    public Type TargetType { get; set; }

    /// <summary>
    /// Overridden data
    /// </summary>
    public T Model { get; set; }
  }
}
