namespace zero.Web.Models
{
  public abstract class ListModel<T> where T : ZeroIdEntity
  {
    /// <summary>
    /// Id of the entity
    /// </summary>
    public string Id { get; set; }
  }
}
