namespace Mixtape.Models;

public interface ISupportsFlavors
{
  /// <summary>
  /// Id of the entity
  /// </summary>
  string Id { get; set; }

  /// <summary>
  /// Alias of the used flavor
  /// </summary>
  string Flavor { get; set; }
}