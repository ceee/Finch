namespace zero.Core.Entities
{
  public interface IRoutedEntity
  {
    /// <summary>
    /// Url for this entity
    /// </summary>
    public string Url { get; set; }
  }
}
