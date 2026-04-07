using Microsoft.Extensions.FileProviders;

namespace Finch.FileStorage;

public class PhysicalFileMeta : IFileMeta
{
  /// <inheritdoc />
  public string Name { get; }

  /// <inheritdoc />
  public string Path { get; }

  /// <inheritdoc />
  public string AbsolutePath { get; }
  
  /// <inheritdoc />
  public string DirectoryPath { get; }

  /// <inheritdoc />
  public long Length { get; }

  /// <inheritdoc />
  public DateTimeOffset LastModifiedDate { get; }

  /// <inheritdoc />
  public bool IsDirectory { get; }

  /// <inheritdoc />
  public Dictionary<string, object> Properties { get; }


  public PhysicalFileMeta(IFileInfo fileInfo, string path)
  {
    Path = path;
    AbsolutePath = fileInfo.PhysicalPath;
    Name = fileInfo.Name;
    DirectoryPath = path.Substring(0, path.Length - fileInfo.Name.Length).TrimEnd('/');
    LastModifiedDate = fileInfo.LastModified;
    Length = fileInfo.Length;
    IsDirectory = fileInfo.IsDirectory;
    Properties = new();
  }
}