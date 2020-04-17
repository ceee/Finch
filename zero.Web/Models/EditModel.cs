namespace zero.Web.Models
{
  public abstract class EditModel
  {
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
