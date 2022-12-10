namespace zero.Media;

public class MediaFileSystem : PhysicalFileSystem, IMediaFileSystem
{
  protected string PublicPathPrefix { get; }


  public MediaFileSystem(string root, string publicPathPrefix) : base(root)
  {
    PublicPathPrefix = (publicPathPrefix ?? String.Empty).EnsureEndsWith('/');
  }


  /// <inheritdoc />
  public override string MapToPublicPath(string path)
  {
    if (path.IsNullOrEmpty())
    {
      return null;
    }
    return PublicPathPrefix + path.TrimStart('/');
  }


  /// <inheritdoc />
  public bool IsMediaPath(string path)
  {
    return path.StartsWith(PublicPathPrefix, StringComparison.InvariantCultureIgnoreCase);
  }
}


public interface IMediaFileSystem : IFileSystem
{
  /// <summary>
  /// Determine whether the given path is part of the media file system
  /// </summary>
  bool IsMediaPath(string path);
}