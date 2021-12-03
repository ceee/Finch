namespace zero.Backoffice;

public class BackofficeAssetFileSystem : PhysicalFileSystem, IBackofficeAssetFileSystem
{
  public BackofficeAssetFileSystem(string root) : base(root) { }
}


public interface IBackofficeAssetFileSystem : IFileSystem
{ 
}