using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <inheritdoc />
  public class MediaFolder : ZeroEntity, IMediaFolder
  {
    /// <inheritdoc />
    public string ParentId { get; set; }

    /// <inheritdoc />
    public bool IsCore { get; set; }
  }


  /// <summary>
  /// A media folder contains media and other folders
  /// </summary>
  [Collection("MediaFolders", LongId = true)]
  public interface IMediaFolder : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Parent folder id
    /// </summary>
    string ParentId { get; set; }

    /// <summary>
    /// Whether this media folder is part of the core database (for multi-tenant setup)
    /// </summary>
    bool IsCore { get; set; }
  }
}