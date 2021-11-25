namespace zero.Media;

public class MediaManagement : IMediaManagement
{
  protected IMediaFileSystem FileSystem { get; set; }

  protected IMediaStore Store { get; set; }


  public MediaManagement(IMediaFileSystem fileSystem, IMediaStore store)
  {
    FileSystem = fileSystem;
    Store = store;
  }


  /// <inheritdoc />
  public virtual async Task<Media> GetFile(string id)
  {
    Media file = await Store.Load(id);
    return file != null && file.Type != MediaType.Folder ? file : null;
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<Media>> UpdateFile(Media file)
  {
    // TODO check new file/image/media
    return await Store.Update(file);
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<Media>> DeleteFile(Media file)
  {
    // TODO delete in file system
    return await Store.Delete(file);
  }


  /// <inheritdoc />
  public virtual async Task<Media> GetFolder(string id)
  {
    Media folder = await Store.Load(id);
    return folder != null && folder.Type == MediaType.Folder ? folder : null;
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<Media>> CreateFolder(Media folder)
  {
    folder.IsActive = true;
    folder.Type = MediaType.Folder;
    return await Store.Create(folder);
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<Media>> CreateFolder(string name, string parentId = null)
  {
    Media media = await Store.Empty();
    media.Name = name;
    media.ParentId = parentId;
    return await CreateFolder(media);
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<Media>> UpdateFolder(Media folder)
  {
    return await Store.Update(folder);
  }


  /// <inheritdoc />
  public virtual async Task<EntityResult<string[]>> DeleteFolder(Media folder)
  {
    // TODO recursive
    return await Store.DeleteWithDescendants(folder);
  }
}


public interface IMediaManagement
{
  /// <summary>
  /// Get a media folder by id
  /// </summary>
  Task<Media> GetFolder(string id);

  /// <summary>
  /// Creates a new media folder
  /// </summary>
  Task<EntityResult<Media>> CreateFolder(Media folder);

  /// <summary>
  /// Creates a new media folder
  /// </summary>
  Task<EntityResult<Media>> CreateFolder(string name, string parentId = null);

  /// <summary>
  /// Rename and store a media folder
  /// </summary>
  Task<EntityResult<Media>> UpdateFolder(Media folder);


  /// <summary>
  /// Deletes a folder, as well as all descendant folders and files
  /// </summary>
  Task<EntityResult<string[]>> DeleteFolder(Media folder);
}
