namespace zero.Core.Entities
{
  /// <summary>
  /// A media folder contains media and other folders
  /// </summary>
  public class MediaFolder : DatabaseEntity
  {
    /// <summary>
    /// Parent folder id
    /// </summary>
    public string ParentId { get; set; }
  }
}