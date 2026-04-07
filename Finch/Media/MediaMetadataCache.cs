using Microsoft.Extensions.Caching.Memory;

namespace Finch.Media;

public sealed class MediaMetadataCache
{
  public IMemoryCache Cache { get; internal set; } = new MemoryCache(new MemoryCacheOptions
  {
    SizeLimit = 1000
  });
}
