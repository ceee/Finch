using System;

namespace zero.Web.Models
{
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
    public EditMetaModel Meta { get; set; } = new EditMetaModel();
  }


  public class EditMetaModel
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


  public abstract class ObsoleteEditModel
  {
    /// <summary>
    /// Id of the entity
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Whether this entity is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Whether this entity can be edited or only viewed
    /// </summary>
    public bool CanEdit { get; set; }

    /// <summary>
    /// Date of creation
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }

    /// <summary>
    /// Meta data for the entity
    /// </summary>
    public EditModelMeta Meta { get; set; } = new EditModelMeta();
  }


  public class EditModelMeta
  {
    /// <summary>
    /// The change token maps to a database entity which holds ID and collection of the model to edit
    /// If these values do not match the entity on save it is rejected
    /// // TODO expiration expiry  session.Advanced.GetMetadataFor(user)[Raven.Client.Constants.Documents.Metadata.Expires] = DateTime.UtcNow.AddMinutes(60);
    /// </summary>
    public string Token { get; set; }
  }
}
