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
    return PublicPathPrefix + path.TrimStart('/');
  }
}


public interface IMediaFileSystem : IFileSystem { }