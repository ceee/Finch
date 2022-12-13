using Microsoft.AspNetCore.Http;
using System.IO;

namespace zero.Media
{
  public static class MediaManagementExtensions
  {
    /// <summary>
    /// Get publicly accessible file path for a media file
    /// </summary>
    /// <param name="mediaId">ID of a media file</param>
    /// <param name="thumbnailKey">An optional thumbnail key which returns the path to a generated thumbnail</param>
    public static async Task<string> GetPublicFilePath(this IMediaManagement media, string mediaId, string thumbnailKey = null)
    {
      Media file = await media.GetFile(mediaId);
      if (file == null)
      {
        return null;
      }
      return media.GetPublicFilePath(file, thumbnailKey);
    }

    /// <summary>
    /// Get file stream for a media file (stream has to be disposed manually)
    /// </summary>
    public static async Task<Stream> GetFileStream(this IMediaManagement media, string mediaId, string thumbnailKey = null)
    {
      Media file = await media.GetFile(mediaId);
      if (file == null)
      {
        return null;
      }
      return await media.GetFileStream(file, thumbnailKey);
    }

    /// <summary>
    /// Uploads a file and persists it
    /// </summary>
    public static async Task<Result<Media>> UploadFile(this IMediaManagement media, IFormFile formFile, string folderId = null, Action<Media> onBeforeSave = null, CancellationToken cancellationToken = default)
    {
      using Stream stream = formFile.OpenReadStream();
      return await media.UploadFile(stream, formFile.FileName, folderId, onBeforeSave, cancellationToken);
    }


    /// <summary>
    /// Uploads a file and persists it
    /// </summary>
    public static async Task<Result<Media>> UploadFile(this IMediaManagement media, byte[] fileBytes, string filename, string folderId = null, Action<Media> onBeforeSave = null, CancellationToken cancellationToken = default)
    {
      using Stream stream = new MemoryStream(fileBytes);
      return await media.UploadFile(stream, filename, folderId, onBeforeSave, cancellationToken);
    }


    /// <summary>
    /// Rename and store a media folder
    /// </summary>
    public static async Task<Result<Media>> RenameFolder(this IMediaManagement media, Media folder, string newName)
    {
      folder.Name = newName;
      return await media.UpdateFolder(folder);
    }


    /// <summary>
    /// Rename and store a media folder
    /// </summary>
    public static async Task<Result<Media>> RenameFolder(this IMediaManagement media, string folderId, string newName)
    {
      Media folder = await media.GetFolder(folderId);
      if (folder == null)
      {
        return Result<Media>.Fail("@errors.idnotfound");
      }

      return await RenameFolder(media, folder, newName);
    }


    /// <summary>
    /// Move a media folder to a new parent
    /// </summary>
    public static async Task<Result<Media>> MoveFolder(this IMediaManagement media, Media folder, string newParentId)
    {
      folder.ParentId = newParentId;
      return await media.UpdateFolder(folder);
    }


    /// <summary>
    /// Move a media folder to a new parent
    /// </summary>
    public static async Task<Result<Media>> MoveFolder(this IMediaManagement media, string folderId, string newParentId)
    {
      Media folder = await media.GetFolder(folderId);
      if (folder == null)
      {
        return Result<Media>.Fail("@errors.idnotfound");
      }

      return await MoveFolder(media, folder, newParentId);
    }


    /// <summary>
    /// Deletes a folder by id
    /// </summary>
    // public static async Task<Result<string[]>> DeleteFolder(this IMediaManagement media, string folderId)
    // {
    //   Media folder = await media.GetFolder(folderId);
    //   if (folder == null)
    //   {
    //     return Result<string[]>.Fail("@errors.idnotfound");
    //   }
    //
    //   return await media.DeleteFolder(folder);
    // }
  }
}
