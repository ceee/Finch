namespace zero.Backoffice;

public class BackofficeResourceFileSystem : PhysicalFileSystem, IBackofficeResourceFileSystem
{
  public BackofficeResourceFileSystem(string root) : base(root) { }
}


public interface IBackofficeResourceFileSystem : IFileSystem
{ 
}