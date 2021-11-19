using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <summary>
  /// A media folder contains media and other folders
  /// </summary>
  [Collection("MediaFolders")]
  public class MediaFolder : ZeroEntity
  {
    /// <summary>
    /// Parent folder id
    /// </summary>
    public string ParentId { get; set; }
  }
}