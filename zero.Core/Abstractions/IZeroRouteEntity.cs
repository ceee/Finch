namespace zero;

public interface IZeroRouteEntity
{
  /// <summary>
  /// Id of the entity
  /// </summary>
  string Id { get; set; }

  /// <summary>
  /// Unique hash for this entity (primarily used for routing)
  /// </summary>
  string Hash { get; set; }
}