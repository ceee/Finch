namespace Mixtape.Models;

public interface ISupportsSorting
{
  /// <summary>
  /// Sort order
  /// </summary>
  uint Sort { get; set; }
}