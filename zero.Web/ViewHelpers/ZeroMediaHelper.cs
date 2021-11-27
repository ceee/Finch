using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Extensions;
using zero.Persistence;

namespace zero.Web.ViewHelpers;

public class ZeroMediaHelper : IZeroMediaHelper
{
  IZeroStore Store;

  public IZeroMediaHelper Core { get; private set; }

  protected bool Global { get; set; }

  /// <summary>
  /// Media cache for repetitive queries within an HTTP request
  /// </summary>
  ConcurrentDictionary<string, Media.Media> Cache { get; set; } = new();


  public ZeroMediaHelper(IZeroStore store, bool global = false)
  {
    Store = store;

    if (!global)
    {
      Core = new ZeroMediaHelper(store, true);
    }
  }


  /// <inheritdoc />
  public async Task<Media.Media> GetById(string id)
  {
    if (id.IsNullOrEmpty())
    {
      return null;
    }

    if (!Cache.TryGetValue(id, out Media.Media media))
    {
      media = await Store.Session(Global).LoadAsync<Media.Media>(id);
      Cache.TryAdd(id, media);
    }

    return media;
  }


  /// <inheritdoc />
  public async Task<Dictionary<string, Media.Media>> GetByIds(string[] ids)
  {
    HashSet<string> remoteIds = new HashSet<string>();
    Dictionary<string, Media.Media> items = new Dictionary<string, Media.Media>();

    foreach (string id in ids)
    {
      if (Cache.TryGetValue(id, out Media.Media media))
      {
        items.TryAdd(id, media);
      }
      else
      {
        remoteIds.Add(id);
      }
    }

    if (remoteIds.Count > 0)
    {
      Dictionary<string, Media.Media> remoteItems = await Store.Session(Global).LoadAsync<Media.Media>(remoteIds);

      foreach (var item in remoteItems)
      {
        items.TryAdd(item.Key, item.Value);
        Cache.TryAdd(item.Key, item.Value);
      }
    }

    return items;
  }


  /// <inheritdoc />
  public async Task<string> GetUrl(string id, bool isAbsolute = false)
  {
    Media.Media media = await GetById(id);
    return media?.Path.TrimStart("url://");
  }


  /// <inheritdoc />
  public async Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false)
  {
    Dictionary<string, Media.Media> medias = await GetByIds(ids);
    return medias.ToDictionary(x => x.Key, x => x.Value?.Path.TrimStart("url://"));
  }
}


public interface IZeroMediaHelper
{
  IZeroMediaHelper Core { get; }

  /// <summary>
  /// Get media by Id
  /// </summary>
  Task<Media.Media> GetById(string id);

  /// <summary>
  /// Get media items by Ids
  /// </summary>
  Task<Dictionary<string, Media.Media>> GetByIds(string[] ids);

  /// <summary>
  /// Get source for a media item
  /// </summary>
  Task<string> GetUrl(string id, bool isAbsolute = false);

  /// <summary>
  /// Get source for media items
  /// </summary>
  Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false);
}