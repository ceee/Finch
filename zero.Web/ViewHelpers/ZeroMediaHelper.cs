using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Extensions;
using zero.Media;
using zero.Persistence;

namespace zero.Web.ViewHelpers;

public class ZeroMediaHelper : IZeroMediaHelper
{
  IZeroStore Store;

  public IZeroMediaHelper Core { get; private set; }

  protected IMediaManagement MediaManagement { get; private set; }

  protected bool Global { get; set; }

  /// <summary>
  /// Media cache for repetitive queries within an HTTP request
  /// </summary>
  ConcurrentDictionary<string, Media.Media> Cache { get; set; } = new();


  public ZeroMediaHelper(IZeroStore store, IMediaManagement mediaManagement, bool global = false)
  {
    Store = store;
    MediaManagement = mediaManagement;

    if (!global)
    {
      Core = new ZeroMediaHelper(store, MediaManagement, true);
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

      if (media != null)
      {
        media.Url = MediaManagement.GetPublicFilePath(media);
      }

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
      if (id.IsNullOrEmpty())
      {
        continue;
      }

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

      foreach ((string id, Media.Media media) in remoteItems)
      {
        if (media != null)
        {
          media.Url = MediaManagement.GetPublicFilePath(media);
        }

        items.TryAdd(id, media);
        Cache.TryAdd(id, media);
      }
    }

    IEnumerable<string> failedIds = ids.Where(x => x.HasValue()).Where(id => !items.ContainsKey(id));

    foreach (string id in failedIds)
    {
      items.Add(id, null);
    }

    return items;
  }


  /// <inheritdoc />
  public async Task<string> GetUrl(string id, bool isAbsolute = false)
  {
    Media.Media media = await GetById(id);

    if (media == null)
    {
      return null;
    }

    if (media.Path.StartsWith("url://"))
    {
      return media.Path.TrimStart("url://");
    }

    return MediaManagement.GetPublicFilePath(media);
  }


  /// <inheritdoc />
  public async Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false)
  {
    Dictionary<string, string> result = new();
    Dictionary<string, Media.Media> medias = await GetByIds(ids);

    foreach ((string id, Media.Media media) in medias)
    {
      if (media == null)
      {
        result.Add(id, null);
      }
      else if (media.Path.StartsWith("url://"))
      {
        result.Add(id, media.Path.TrimStart("url://"));
      }
      else
      {
        result.Add(id, MediaManagement.GetPublicFilePath(media));
      }
    }

    return result;
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