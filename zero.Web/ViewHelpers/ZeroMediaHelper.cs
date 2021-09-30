using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Web.ViewHelpers
{
  public class ZeroMediaHelper : IZeroMediaHelper
  {
    IZeroDocumentSession Session;

    public IZeroMediaHelper Core { get; private set; }

    /// <summary>
    /// Media cache for repetitive queries within an HTTP request
    /// </summary>
    ConcurrentDictionary<string, Media> Cache { get; set; } = new();


    public ZeroMediaHelper(IZeroDocumentSession session, IZeroCoreDocumentSession coreSession, bool isCore = false)
    {
      Session = session;

      if (!isCore)
      {
        Core = new ZeroMediaHelper(coreSession, coreSession, true);
      }
    }


    /// <inheritdoc />
    public async Task<Media> GetById(string id)
    {
      if (id.IsNullOrEmpty())
      {
        return null;
      }

      if (!Cache.TryGetValue(id, out Media media))
      {
        media = await Session.LoadAsync<Media>(id);
        Cache.TryAdd(id, media);
      }

      return media;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, Media>> GetByIds(string[] ids)
    {
      HashSet<string> remoteIds = new HashSet<string>();
      Dictionary<string, Media> items = new Dictionary<string, Media>();

      foreach (string id in ids)
      {
        if (Cache.TryGetValue(id, out Media media))
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
        Dictionary<string, Media> remoteItems = await Session.LoadAsync<Media>(remoteIds);

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
      Media media = await GetById(id);
      return media?.Source.TrimStart("url://");
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false)
    {
      Dictionary<string, Media> medias = await GetByIds(ids);
      return medias.ToDictionary(x => x.Key, x => x.Value?.Source.TrimStart("url://"));
    }
  }


  public interface IZeroMediaHelper
  {
    IZeroMediaHelper Core { get; }

    /// <summary>
    /// Get media by Id
    /// </summary>
    Task<Media> GetById(string id);

    /// <summary>
    /// Get media items by Ids
    /// </summary>
    Task<Dictionary<string, Media>> GetByIds(string[] ids);

    /// <summary>
    /// Get source for a media item
    /// </summary>
    Task<string> GetUrl(string id, bool isAbsolute = false);

    /// <summary>
    /// Get source for media items
    /// </summary>
    Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false);
  }
}
