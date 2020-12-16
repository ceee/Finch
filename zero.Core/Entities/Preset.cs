using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public interface IPreset
  {
    string Alias { get; set; }
  }


  public class PresetOverride<T> : ZeroEntity, IPresetOverride<T>
  {
    /// <inheritdoc />
    public T Model { get; set; }
  }


  [Collection("Presets")]
  public interface IPresetOverride<T> : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Overridden data
    /// </summary>
    T Model { get; set; }
  }
}
