using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace zero;

[DebuggerDisplay("Id = {Id,nq}, Name = {Name}, Alias = {Alias}")]
public class ZeroEntity : ZeroIdEntity, IZeroDbConventions, IZeroRouteEntity
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
  /// Configuration of the base entity (which this one inherits from)
  /// </summary>
  public BlueprintConfiguration Blueprint { get; set; }

  /// <summary>
  /// Language of the entity
  /// </summary>
  public string LanguageId { get; set; }

  /// <summary>
  /// [Warning] This field is always empty when bound to the database.
  /// It is only filled in the app-code for routing.
  /// </summary>
  [JsonIgnore]
  public string Url { get; set; }
}