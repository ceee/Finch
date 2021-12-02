namespace zero.Api.Models;


public abstract class DisplayModel<T> : ZeroIdEntity, ISupportsFlavors where T : ZeroEntity
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
  /// Unique hash for this entity (primarily used for routing)
  /// </summary>
  public string Hash { get; set; }

  /// <summary>
  /// Backoffice user who last modified this content
  /// </summary>
  public string LastModifiedById { get; set; }

  /// <summary>
  /// Date of last modification
  /// </summary>
  public DateTimeOffset LastModifiedDate { get; set; }

  /// <summary>
  /// Backoffice user who created this content
  /// </summary>
  public string CreatedById { get; set; }

  /// <summary>
  /// Date of creation
  /// </summary>
  public DateTimeOffset CreatedDate { get; set; }

  /// <summary>
  /// Language of the entity
  /// </summary>
  public string LanguageId { get; set; }

  /// <summary>
  /// Configured flavor of this entity
  /// </summary>
  public string Flavor { get; set; }
}