namespace zero.Media
{
  public static class MediaManagementExtensions
  {
    /// <summary>
    /// Rename and store a media folder
    /// </summary>
    public static async Task<EntityResult<Media>> RenameFolder(this IMediaManagement media, Media folder, string newName)
    {
      folder.Name = newName;
      return await media.UpdateFolder(folder);
    }


    /// <summary>
    /// Rename and store a media folder
    /// </summary>
    public static async Task<EntityResult<Media>> RenameFolder(this IMediaManagement media, string folderId, string newName)
    {
      Media folder = await media.GetFolder(folderId);
      if (folder == null)
      {
        return EntityResult<Media>.Fail("@errors.idnotfound");
      }

      return await RenameFolder(media, folder, newName);
    }


    /// <summary>
    /// Move a media folder to a new parent
    /// </summary>
    public static async Task<EntityResult<Media>> MoveFolder(this IMediaManagement media, Media folder, string newParentId)
    {
      folder.ParentId = newParentId;
      return await media.UpdateFolder(folder);
    }


    /// <summary>
    /// Move a media folder to a new parent
    /// </summary>
    public static async Task<EntityResult<Media>> MoveFolder(this IMediaManagement media, string folderId, string newParentId)
    {
      Media folder = await media.GetFolder(folderId);
      if (folder == null)
      {
        return EntityResult<Media>.Fail("@errors.idnotfound");
      }

      return await MoveFolder(media, folder, newParentId);
    }


    /// <summary>
    /// Deletes a folder by id
    /// </summary>
    public static async Task<EntityResult<Media>> DeleteFolder(this IMediaManagement media, string folderId)
    {
      Media folder = await media.GetFolder(folderId);
      if (folder == null)
      {
        return EntityResult<Media>.Fail("@errors.idnotfound");
      }

      return await media.DeleteFolder(folder);
    }
  }
}
