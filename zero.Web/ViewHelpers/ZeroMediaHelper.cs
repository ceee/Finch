using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;
using zero.Core.Extensions;
using System.Collections.Concurrent;

namespace zero.Web.ViewHelpers
{
  public class ZeroMediaHelper : IZeroMediaHelper
  {
    HttpContext HttpContext;

    IMediaApi MediaApi;

    /// <summary>
    /// Media cache for repetitive queries within an HTTP request
    /// </summary>
    ConcurrentDictionary<string, Media> Cache { get; set; } = new();


    public ZeroMediaHelper(IHttpContextAccessor httpContextAccessor, IMediaApi mediaApi)
    {
      HttpContext = httpContextAccessor.HttpContext;
      MediaApi = mediaApi;
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
        media = await MediaApi.GetById(id);
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
        Dictionary<string, Media> remoteItems = await MediaApi.GetById(remoteIds);

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
