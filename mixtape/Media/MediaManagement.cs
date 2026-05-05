using System.IO;

namespace Mixtape.Media;

public class MediaManagement : IMediaManagement
{
  protected IMediaFileSystem FileSystem { get; set; }

  protected IMixtapeMediaStoreDbProvider Db { get; set; }

  protected IMediaCreator Creator { get; set; }


  public MediaManagement(IMediaFileSystem fileSystem, IMixtapeMediaStoreDbProvider db, IMediaCreator creator)
  {
    FileSystem = fileSystem;
    Db = db;
    Creator = creator;
  }



  /// <inheritdoc />
  public virtual string GetPublicFilePath(Media file, string thumbnailKey = null)
  {
    string path = file?.Path;

    if (!thumbnailKey.IsNullOrWhiteSpace())
    {
      path = file?.Thumbnails?.GetValueOrDefault(thumbnailKey);
    }

    if (path.IsNullOrEmpty())
    {
      return null;
    }

    return FileSystem.MapToPublicPath(path);
  }


  /// <inheritdoc />
  public virtual async Task<Stream> GetFileStream(Media file, string thumbnailKey = null)
  {
    if (file == null)
    {
      return null;
    }

    string path = file?.Path;

    if (!thumbnailKey.IsNullOrWhiteSpace())
    {
      path = file?.Thumbnails?.GetValueOrDefault(thumbnailKey);
    }

    if (path.IsNullOrEmpty())
    {
      return null;
    }

    return await FileSystem.StreamFile(path);
  }


  /// <inheritdoc />
  public virtual async Task<Media> GetFile(string id)
  {

    Media file = await Db.Find<Media>(x => x.Id == id);
    return file != null && !file.IsFolder ? file : null;
  }


  /// <inheritdoc />
  public virtual async Task<Result<Media>> UpdateFile(Media file)
  {
    // TODO check new file/image/media
    return await Db.Update(file);
  }


  /// <inheritdoc />
  public virtual async Task<Result<Media>> DeleteFile(Media file)
  {
    if (file == null)
    {
      return Result<Media>.Success();
    }
    await FileSystem.DeleteDirectory(file.FileId, true);
    return await Db.Delete(file);
  }



  /// <inheritdoc />
  public virtual async Task<Result<Media>> UploadFile(Stream fileStream, string filename, string folderId = null, Action<Media> onBeforeSave = null, bool persist = true, CancellationToken cancellationToken = default)
  {
    Result<Media> result = await Creator.UploadFile(fileStream, filename, folderId, cancellationToken);

    if (!result.IsSuccess || !persist)
    {
      return result;
    }

    onBeforeSave?.Invoke(result.Model);

    return await Db.Create(result.Model);
  }


  /// <inheritdoc />
  public virtual async Task<Media> GetFolder(string id)
  {
    Media folder = await Db.Find<Media>(x => x.Id == id);
    return folder != null && folder.IsFolder ? folder : null;
  }


  /// <inheritdoc />
  public virtual async Task<Result<Media>> CreateFolder(Media folder)
  {
    folder.IsActive = true;
    folder.IsFolder = true;
    return await Db.Create(folder);
  }


  /// <inheritdoc />
  public virtual async Task<Result<Media>> CreateFolder(string name, string parentId = null)
  {
    Media media = new();
    media.Name = name;
    media.ParentId = parentId;
    return await CreateFolder(media);
  }


  /// <inheritdoc />
  public virtual async Task<Result<Media>> UpdateFolder(Media folder)
  {
    return await Db.Update(folder);
  }


  /// <inheritdoc />
  // public virtual async Task<Result<string[]>> DeleteFolder(Media folder)
  // {
  //   // TODO recursive
  //   return await Store.DeleteWithDescendants(folder);
  // }
}


public interface IMediaManagement
{
  /// <summary>
  /// Get publicly accessible file path for a media file
  /// </summary>
  /// <param name="file">The media file</param>
  /// <param name="thumbnailKey">An optional thumbnail key which returns the path to a generated thumbnail</param>
  string GetPublicFilePath(Media file, string thumbnailKey = null);

  /// <summary>
  /// Get file stream for a media file (stream has to be disposed manually)
  /// </summary>
  Task<Stream> GetFileStream(Media file, string thumbnailKey = null);

  /// <summary>
  /// Get a media file by id
  /// </summary>
  Task<Media> GetFile(string id);

  /// <summary>
  /// Update and store a media file
  /// </summary>
  Task<Result<Media>> UpdateFile(Media file);

  /// <summary>
  /// Deletes a media file (collection entry as well as physical file)
  /// </summary>
  Task<Result<Media>> DeleteFile(Media file);

  /// <summary>
  /// Uploads a file and persists it
  /// </summary>
  Task<Result<Media>> UploadFile(Stream fileStream, string filename, string folderId = null, Action<Media> onBeforeSave = null, bool persist = true, CancellationToken cancellationToken = default);

  /// <summary>
  /// Get a media folder by id
  /// </summary>
  Task<Media> GetFolder(string id);

  /// <summary>
  /// Creates a new media folder
  /// </summary>
  Task<Result<Media>> CreateFolder(Media folder);

  /// <summary>
  /// Creates a new media folder
  /// </summary>
  Task<Result<Media>> CreateFolder(string name, string parentId = null);

  /// <summary>
  /// Rename and store a media folder
  /// </summary>
  Task<Result<Media>> UpdateFolder(Media folder);

  /// <summary>
  /// Deletes a folder, as well as all descendant folders and files
  /// </summary>
  //Task<Result<string[]>> DeleteFolder(Media folder);
}
