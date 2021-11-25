namespace zero.FileStorage;

public interface IFileMeta
{
  /// <summary>
  /// Full path of the item within the file system
  /// </summary>
  string Path { get; }

  /// <summary>
  /// Absolute path of the item
  /// </summary>
  string AbsolutePath { get; }

  /// <summary>
  /// Name of the item
  /// </summary>
  string Name { get; }

  /// <summary>
  /// Full path to the directory containing the item
  /// </summary>
  string DirectoryPath { get; }

  /// <summary>
  /// Length of the file in bytes
  /// </summary>
  long Length { get; }

  /// <summary>
  /// Date when the item was last modified
  /// </summary>
  DateTimeOffset LastModifiedDate { get; }

  /// <summary>
  /// Whether this item is directory or a file
  /// </summary>
  bool IsDirectory { get; }

  /// <summary>
  /// Additional properties
  /// </summary>
  Dictionary<string, object> Properties { get; }
}
