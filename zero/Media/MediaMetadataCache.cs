using Microsoft.Extensions.Caching.Memory;

namespace zero.Media;

public sealed class MediaMetadataCache
{
  public IMemoryCache Cache { get; internal set; } = new MemoryCache(new MemoryCacheOptions
  {
    SizeLimit = 1000
  });
}
