namespace zero.Media;

public class WebRootFileSystem : PhysicalFileSystem, IWebRootFileSystem
{
  protected string PublicPathPrefix { get; }


  public WebRootFileSystem(string root, string publicPathPrefix) : base(root)
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
}


public interface IWebRootFileSystem : IFileSystem
{
}