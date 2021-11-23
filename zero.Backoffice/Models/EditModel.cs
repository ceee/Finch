namespace zero.Backoffice.Models;

public class EditModel : EditModel<object> { }

public class EditModel<T>
{
  /// <summary>
  /// Model
  /// </summary>
  public T Entity { get; set; }

  /// <summary>
  /// Meta data
  /// </summary>
  public EditMetaModel Meta { get; set; } = new();
  
  /// <summary>
  /// Permissions for this entity
  /// </summary>
  public EditPermissionModel Permissions { get; set; } = new();
}


public class EditMetaModel
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


public class EditPermissionModel
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