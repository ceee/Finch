namespace zero.Backoffice.Models;

public abstract class DisplayModel<T> : ZeroIdEntity where T : ZeroIdEntity
{
  /// <summary>
  /// Meta data
  /// </summary>
  public DisplayModelMeta Meta { get; set; } = new();
  
  /// <summary>
  /// Permissions for this entity
  /// </summary>
  public DisplayModelPermissions Permissions { get; set; } = new();
}


public class DisplayModelMeta
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