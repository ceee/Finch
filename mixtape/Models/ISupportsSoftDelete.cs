namespace Mixtape.Models;

public interface ISupportsSoftDelete
{
  /// <summary>
  /// Whether the entity has been deleted
  /// </summary>
  bool IsDeleted { get; set; }
}