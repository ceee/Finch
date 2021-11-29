namespace zero.Api.Models;

public abstract class DisplayModel : ZeroIdEntity
{
  /// <summary>
  /// Meta data
  /// </summary>
  public DisplayModelConfiguration Configuration { get; set; } = new();
  
  /// <summary>
  /// Permissions for this entity
  /// </summary>
  public DisplayModelPermissions Permissions { get; set; } = new();
}


public abstract class DisplayModel<T> : DisplayModel where T : ZeroEntity
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
}


public class DisplayModelConfiguration
{
  /// <summary>
  /// Wehther this entity is application aware
  /// </summary>
  public bool IsAppAware { get; set; }

  /// <summary>
  /// Whether this entity can be shared across applications (only for IsAppAware=true)
  /// </summary>
  public bool CanBeShared { get; set; }

  public bool IsShared { get; set; }

  /// <summary>
  /// The change token maps to a database entity which holds ID and collection of the model to edit
  /// If these values do not match the entity on save it is rejected
  /// // TODO expiration expiry  session.Advanced.GetMetadataFor(user)[Raven.Client.Constants.Documents.Metadata.Expires] = DateTime.UtcNow.AddMinutes(60);
  /// </summary>
  public string Token { get; set; }
}


public class DisplayModelPermissions
{
  /// <summary>
  /// Whether an entity of this type can be created
  /// </summary>
  public bool CanCreate { get; set; }

  /// <summary>
  /// Whether an entity of this type can be created in the shared app space
  /// </summary>
  public bool CanCreateShared { get; set; }

  /// <summary>
  /// Whether this entity can be edited or only viewed
  /// </summary>
  public bool CanEdit { get; set; }

  /// <summary>
  /// Whether this entity can be deleted
  /// </summary>
  public bool CanDelete { get; set; }
}