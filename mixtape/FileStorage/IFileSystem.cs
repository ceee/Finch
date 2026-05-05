using System.IO;

namespace Mixtape.FileStorage;

public interface IFileSystem
{
  /// <summary>
  /// Map a relative path to an absolute internal path 
  /// </summary>
  string Map(string path);

  /// <summary>
  /// Map an internal path to a public accessible path
  /// </summary>
  string MapToPublicPath(string path);

  /// <summary>
  /// Determines whether a file or directory at the given path already exists
  /// </summary>
  Task<bool> Exists(string path, CancellationToken cancellationToken = default);

  /// <summary>
  /// Get information of a file
  /// </summary>
  Task<IFileMeta> GetFileInfo(string path, CancellationToken cancellationToken = default);

  /// <summary>
  /// Returns a readable stream for a file
  /// </summary>
  Task<Stream> StreamFile(string path, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates a file at the given path
  /// </summary>
  Task CreateFile(string path, Stream fileStream, bool overwrite = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// Deletes a file at the given path. This method returns if the file does not exist.
  /// </summary>
  Task DeleteFile(string path, CancellationToken cancellationToken = default);

  /// <summary>
  /// Moves a file to another path (can also be used to rename resources)
  /// </summary>
  Task MoveFile(string oldPath, string newPath, CancellationToken cancellationToken = default);

  /// <summary>
  /// Copies a file to another path
  /// </summary>
  Task CopyFile(string oldPath, string newPath, CancellationToken cancellationToken = default);

  /// <summary>
  /// Get information of a directory
  /// </summary>
  Task<IFileMeta> GetDirectoryInfo(string path, CancellationToken cancellationToken = default);

  /// <summary>
  /// Get all items within a directory
  /// </summary>
  IEnumerable<IFileMeta> GetDirectoryContent(string path = null, bool recursive = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// Tries to create a directory at the given path. This method returns if the directory already exists.
  /// </summary>
  Task CreateDirectory(string path, CancellationToken cancellationToken = default);

  /// <summary>
  /// Deletes a directory at the given path. This method returns if the directory does not exist.
  /// </summary>
  Task DeleteDirectory(string path, bool recursive = false, CancellationToken cancellationToken = default);
}