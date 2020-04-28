namespace zero.Web.Models
{
  public abstract class EditModel
  {
    /// <summary>
    /// Id of the entity
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Whether this entity can be edited or only viewed
    /// </summary>
    public bool CanEdit { get; set; }

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
