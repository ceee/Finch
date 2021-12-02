namespace zero.Api.Models;

public abstract class SaveModel<T> : ZeroIdEntity, ISupportsFlavors where T : ZeroEntity
{
  /// <summary>
  /// Full name of the entity
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Alias (non-unique) which can be used in the frontend and URLs
  /// </summary>
  public string Alias { get; set; }

  /// <summary>
  /// A key which can be used to query this entity in code
  /// </summary>
  public string Key { get; set; }

  /// <summary>
  /// Sort order
  /// </summary>
  public uint Sort { get; set; }

  /// <summary>
  /// Whether the entity is visible in the frontend
  /// </summary>
  public bool IsActive { get; set; }

  /// <summary>
  /// Configured flavor of this entity
  /// </summary>
  public string Flavor { get; set; }
}