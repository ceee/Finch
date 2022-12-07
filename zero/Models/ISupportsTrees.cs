namespace zero.Models;

public interface ISupportsTrees
{
  /// <summary>
  /// Id of the entity
  /// </summary>
  string Id { get; set; }

  /// <summary>
  /// Parent id
  /// </summary>
  string ParentId { get; set; }

  /// <summary>
  /// Sort order
  /// </summary>
  uint Sort { get; set; }
}