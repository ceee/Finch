namespace zero.Media;

/// <summary>
/// A cached media entity comes from the IMemoryCache
/// and is therefore reduced to only necessary properties
/// (Id, ParentId, Path, Metadata, AltText, Caption)
/// </summary>
public sealed class CachedMedia : Media
{
  internal static CachedMedia Create(MediaCacheEntry entry)
  {
    return new()
    {
      Id = entry.Id,
      ParentId = entry.ParentId,
      Path = entry.Path,
      AltText = entry.AltText,
      Caption = entry.Caption,
      Metadata = entry.Metadata
    };
  }
}


internal class MediaCacheEntry
{
  /// <summary>
  /// Id of the entity
  /// </summary>
  public string Id { get; set; }
  
  /// <summary>
  /// Id of the parent folder
  /// </summary>
  public string ParentId { get; set; }

  /// <summary>
  /// Path of the media item
  /// </summary>
  public string Path { get; set; }
  
  /// <summary>
  /// Alternative text which is used when the media can't be loaded
  /// </summary>
  public string AltText { get; set; }
  
  /// <summary>
  /// Additional caption text
  /// </summary>
  public string Caption { get; set; }
  
  /// <summary>
  /// Meta data for images/videos
  /// </summary>
  public MediaMetadata Metadata { get; set; }
  
  
  internal static MediaCacheEntry Create(Media entry)
  {
    return new()
    {
      Id = entry.Id,
      ParentId = entry.ParentId,
      Path = entry.Path,
      AltText = entry.AltText,
      Caption = entry.Caption,
      Metadata = entry.Metadata
    };
  }
}