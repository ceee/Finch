using Microsoft.Extensions.FileProviders.Physical;
using System.IO;

namespace zero.FileStorage;

public class PhysicalFileSystem : IFileSystem
{
  readonly string basePath;


  public PhysicalFileSystem(string basePath)
  {
    this.basePath = basePath;
  }


  /// <inheritdoc />
  public Task<IFileMeta> GetFileInfo(string path, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedPath = ResolvePath(path);

      PhysicalFileInfo fileInfo = new(new FileInfo(resolvedPath));

      if (!fileInfo.Exists)
      {
        return Task.FromResult<IFileMeta>(default);
      }

      return Task.FromResult<IFileMeta>(new PhysicalFileMeta(fileInfo, path));
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not get file info for path '{path}'", ex);
    }
  }


  /// <inheritdoc />
  public Task<Stream> StreamFile(string path, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedPath = ResolvePath(path);

      if (resolvedPath.IsNullOrEmpty() || !File.Exists(resolvedPath))
      {
        throw new FileSystemException($"The path '{path}' does not exist in the file system");
      }

      return Task.FromResult<Stream>(File.OpenRead(resolvedPath));
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not get file stream for path '{path}'", ex);
    }
  }


  /// <inheritdoc />
  public async Task CreateFile(string path, Stream fileStream, bool overwrite = false, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedPath = ResolvePath(path);

      if ((!overwrite && File.Exists(resolvedPath)) || Directory.Exists(resolvedPath))
      {
        throw new FileSystemException($"A file/directory at the path '{path}' already existsin the file system");
      }

      string directoryPath = Path.GetDirectoryName(resolvedPath);
      Directory.CreateDirectory(directoryPath);

      FileInfo fileInfo = new(resolvedPath);
      using var outputStream = fileInfo.Create();
      await fileStream.CopyToAsync(outputStream, cancellationToken);
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not get file stream for path '{path}'", ex);
    }
  }


  /// <inheritdoc />
  public Task DeleteFile(string path, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedPath = ResolvePath(path);
      File.Delete(resolvedPath);
      return Task.CompletedTask;
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not delete file for path '{path}'", ex);
    }
  }


  /// <inheritdoc />
  public Task MoveFile(string oldPath, string newPath, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedOldPath = ResolvePath(oldPath);
      string resolvedNewPath = ResolvePath(newPath);

      if (!File.Exists(resolvedOldPath))
      {
        throw new FileSystemException($"The path '{oldPath}' does not exist in the file system");
      }

      if (File.Exists(resolvedNewPath) || Directory.Exists(resolvedNewPath))
      {
        throw new FileSystemException($"A file/directory at the path '{newPath}' already exists in the file system");
      }

      File.Move(resolvedOldPath, resolvedNewPath);
      return Task.CompletedTask;
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not move file from path '{oldPath}' to '{newPath}'", ex);
    }
  }


  /// <inheritdoc />
  public Task CopyFile(string oldPath, string newPath, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedOldPath = ResolvePath(oldPath);
      string resolvedNewPath = ResolvePath(newPath);

      if (!File.Exists(resolvedOldPath))
      {
        throw new FileSystemException($"The path '{oldPath}' does not exist in the file system");
      }

      if (File.Exists(resolvedNewPath) || Directory.Exists(resolvedNewPath))
      {
        throw new FileSystemException($"A file/directory at the path '{newPath}' already exists in the file system");
      }

      File.Copy(resolvedOldPath, resolvedNewPath);
      return Task.CompletedTask;
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not copy file from path '{oldPath}' to '{newPath}'", ex);
    }
  }


  /// <inheritdoc />
  public Task<IFileMeta> GetDirectoryInfo(string path, CancellationToken cancellationToken = default) => GetFileInfo(path, cancellationToken);


  /// <inheritdoc />
  public IAsyncEnumerable<IFileMeta> GetDirectoryContent(string path = null, bool recursive = false, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedPath = ResolvePath(path);
      List<IFileMeta> results = new();
      SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

      if (!Directory.Exists(resolvedPath))
      {
        return results.ToAsyncEnumerable();
      }

      results.AddRange(Directory.GetDirectories(resolvedPath, "*", searchOption).Select(f =>
      {
        PhysicalDirectoryInfo fileSystemInfo = new(new DirectoryInfo(f));
        return new PhysicalFileMeta(fileSystemInfo, ResolvePath(f.Substring(basePath.Length)));
      }));

      results.AddRange(Directory.GetFiles(resolvedPath, "*", searchOption).Select(f =>
      {
        PhysicalFileInfo fileSystemInfo = new(new FileInfo(f));
        return new PhysicalFileMeta(fileSystemInfo, ResolvePath(f.Substring(basePath.Length)));
      }));

      return results.ToAsyncEnumerable();
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not get directory content for path '{path}'", ex);
    }
  }


  /// <inheritdoc />
  public Task CreateDirectory(string path, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedPath = ResolvePath(path);

      if (File.Exists(resolvedPath))
      {
        throw new FileSystemException($"A file at the path '{path}' already exists in the file system");
      }

      if (!Directory.Exists(resolvedPath))
      {
        Directory.CreateDirectory(resolvedPath);
      }

      return Task.CompletedTask;
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not create directory at path '{path}'", ex);
    }
  }


  /// <inheritdoc />
  public Task DeleteDirectory(string path, bool recursive = false, CancellationToken cancellationToken = default)
  {
    try
    {
      string resolvedPath = ResolvePath(path);
      Directory.Delete(resolvedPath, recursive);
      return Task.CompletedTask;
    }
    catch (Exception ex) when (ex is not FileSystemException)
    {
      throw new FileSystemException($"Could not delete directory at path '{path}'", ex);
    }
  }


  /// <summary>
  /// Creates a fully-usable absolte path from the given path
  /// </summary>
  protected string ResolvePath(string path)
  {
    try
    {
      if (path.IsNullOrWhiteSpace())
      {
        return basePath;
      }

      // normalize path
      path = path.Replace('\\', '/').FullTrim().Trim('/');

      string physicalPath = Path.Combine(basePath, path);

      // do not allow paths which are outside this file system
      // the boundaries are set be the basePath which is assigned in the file system constructor
      if (!Path.GetFullPath(physicalPath).StartsWith(basePath, StringComparison.InvariantCultureIgnoreCase))
      {
        throw new FileSystemException($"The path '{path}' is outside the file system");
      }

      return physicalPath;
    }
    catch (FileSystemException)
    {
      throw;
    }
    catch (Exception ex)
    {
      throw new FileSystemException($"Could not resolve path '{path}'", ex);
    }
  }
}
