using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <inheritdoc />
  public class MediaFolder : ZeroEntity, IMediaFolder
  {
    /// <inheritdoc />
    public Ref<IMediaFolder> ParentId { get; set; }
  }


  /// <summary>
  /// A media folder contains media and other folders
  /// </summary>
  [Collection("MediaFolders")]
  public interface IMediaFolder : IZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Parent folder id
    /// </summary>
    Ref<IMediaFolder> ParentId { get; set; }
  }
}