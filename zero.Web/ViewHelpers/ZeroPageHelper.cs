using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;
using zero.Core.Extensions;

namespace zero.Web.ViewHelpers
{
  public class ZeroPageHelper : IZeroPageHelper
  {
    HttpContext HttpContext;

    IMediaApi MediaApi;

    /// <summary>
    /// Media cache for repetitive queries within an HTTP request
    /// </summary>
    Dictionary<string, Media> Cache { get; set; } = new Dictionary<string, Media>();


    public ZeroPageHelper(IHttpContextAccessor httpContextAccessor, IMediaApi mediaApi)
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
        Cache.Add(id, media);
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
          items.Add(id, media);
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
          items.Add(item.Key, item.Value);
          Cache.Add(item.Key, item.Value);
        }
      }

      return items;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(string id, bool isAbsolute = false)
    {
      Media media = await GetById(id);
      return media?.Source;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false)
    {
      Dictionary<string, Media> medias = await GetByIds(ids);
      return medias.ToDictionary(x => x.Key, x => x.Value?.Source);
    }
  }


  public interface IZeroPageHelper
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
