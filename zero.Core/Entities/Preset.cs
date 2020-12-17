using System;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public abstract class Preset : IPreset
  {
    /// <inheritdoc />
    public string Key { get; set; }

    /// <inheritdoc />
    public string Name { get; set; }
  }

  public interface IPreset
  {
    /// <summary>
    /// Key is used to query a preset
    /// </summary>
    string Key { get; set; }

    /// <summary>
    /// Name of the preset
    /// </summary>
    string Name { get; set; }
  }


  public class PresetOverride<T> : ZeroEntity, IPresetOverride<T> where T : IPreset
  {
    /// <inheritdoc />
    public string Key { get; set; }

    /// <inheritdoc />
    public Type TargetType { get; set; }

    /// <inheritdoc />
    public T Model { get; set; }
  }


  [Collection("Presets")]
  public interface IPresetOverride<T> : IZeroEntity, IZeroDbConventions where T : IPreset
  {
    /// <summary>
    /// Key is used to query a preset
    /// </summary>
    string Key { get; set; }

    /// <summary>
    /// Type of the target model (used to query)
    /// </summary>
    Type TargetType { get; set; }

    /// <summary>
    /// Overridden data
    /// </summary>
    T Model { get; set; }
  }
}
