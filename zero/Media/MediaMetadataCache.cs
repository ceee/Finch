using Microsoft.Extensions.Caching.Memory;

namespace zero.Media;

public class MediaMetadataCache : IMediaMetadataCache
{
  private const string PREFIX = "zero/media/";

  protected IMemoryCache Cache { get; set; }


  public MediaMetadataCache(IMemoryCache cache)
  {
    Cache = cache;
  }


  /// <inheritdoc />
  public bool TryGet(string id, out CachedMedia media)
  {
    MediaCacheEntry entry = Cache.Get<MediaCacheEntry>(Key(id));

    media = entry != null ? CachedMedia.Create(entry) : null;
    return entry != null;
  }


  /// <inheritdoc />
  public void Set(Media media)
  {
    MediaCacheEntry entry = MediaCacheEntry.Create(media);
    Cache.Set(Key(media.Id), entry, new MemoryCacheEntryOptions()
    {
      SlidingExpiration = TimeSpan.FromSeconds(60 * 60 * 24)
    });
  }


  /// <summary>
  /// Generate cache key from media Id
  /// </summary>
  static string Key(string id)
  {
    return PREFIX + id;
  }
}


public interface IMediaMetadataCache
{
  /// <summary>
  /// Get a cached and reduced version of a media entity
  /// </summary>
  bool TryGet(string id, out CachedMedia media);

  /// <summary>
  /// Store a media entity in cache
  /// </summary>
  void Set(Media media);
}