using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Routing;
using zero.Core.Extensions;
using zero.Core.Collections;

namespace zero.Web.ViewHelpers
{
  public class ZeroMediaHelper : IZeroMediaHelper
  {
    HttpContext HttpContext;

    IMediaCollection MediaCollection;

    /// <summary>
    /// Media cache for repetitive queries within an HTTP request
    /// </summary>
    Dictionary<string, IMedia> Cache { get; set; } = new Dictionary<string, IMedia>();


    public ZeroMediaHelper(IHttpContextAccessor httpContextAccessor, IMediaCollection mediaCollection)
    {
      HttpContext = httpContextAccessor.HttpContext;
      MediaCollection = mediaCollection;
    }


    /// <inheritdoc />
    public async Task<IMedia> GetById(string id)
    {
      if (id.IsNullOrEmpty())
      {
        return null;
      }

      if (!Cache.TryGetValue(id, out IMedia media))
      {
        media = await MediaCollection.GetById(id);
        Cache.Add(id, media);
      }

      return media;
    }


    /// <inheritdoc />
    public async Task<IMedia> GetById(Ref reference)
    {
      if (reference == null)
      {
        return null;
      }

      string id = reference.ToString();

      if (!Cache.TryGetValue(id, out IMedia media))
      {
        await MediaCollection.Scoped(reference.IsCore ? "shared" : null, async() =>
        {
          media = await MediaCollection.GetById(reference.Id);
        });
        Cache.Add(id, media);
      }

      return media;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, IMedia>> GetByIds(string[] ids)
    {
      HashSet<string> remoteIds = new HashSet<string>();
      Dictionary<string, IMedia> items = new Dictionary<string, IMedia>();

      foreach (string id in ids)
      {
        if (Cache.TryGetValue(id, out IMedia media))
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
        Dictionary<string, IMedia> remoteItems = await MediaCollection.GetByIds(remoteIds.ToArray());

        foreach (var item in remoteItems)
        {
          items.Add(item.Key, item.Value);
          Cache.Add(item.Key, item.Value);
        }
      }

      return items;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, IMedia>> GetByIds(Ref[] references)
    {
      HashSet<Ref> remoteIds = new HashSet<Ref>();
      Dictionary<string, IMedia> items = new Dictionary<string, IMedia>();

      foreach (Ref reference in references)
      {
        string id = reference.ToString();

        if (Cache.TryGetValue(id, out IMedia media))
        {
          items.Add(id, media);
        }
        else
        {
          remoteIds.Add(reference);
        }
      }

      if (remoteIds.Count > 0)
      {
        foreach (Ref reference in remoteIds)
        {
          // TODO this is super unperformant as we are switching scopes per request here
          items.Add(reference.Id, await GetById(reference));
        }
      }

      return items;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(string id, bool isAbsolute = false)
    {
      IMedia media = await GetById(id);
      return media?.Source;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(Ref reference, bool isAbsolute = false)
    {
      IMedia media = await GetById(reference);
      return media?.Source;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false)
    {
      Dictionary<string, IMedia> medias = await GetByIds(ids);
      return medias.ToDictionary(x => x.Key, x => x.Value?.Source);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, string>> GetUrls(Ref[] references, bool isAbsolute = false)
    {
      Dictionary<string, IMedia> medias = await GetByIds(references);
      return medias.ToDictionary(x => x.Key, x => x.Value?.Source);
    }
  }


  public interface IZeroMediaHelper
  {
    /// <summary>
    /// Get media by Id
    /// </summary>
    Task<IMedia> GetById(string id);

    /// <summary>
    /// Get media items by Ids
    /// </summary>
    Task<Dictionary<string, IMedia>> GetByIds(string[] ids);

    /// <summary>
    /// Get source for a media item
    /// </summary>
    Task<string> GetUrl(string id, bool isAbsolute = false);

    /// <summary>
    /// Get source for media items
    /// </summary>
    Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false);

    /// <summary>
    /// Get media by Id
    /// </summary>
    Task<IMedia> GetById(Ref reference);

    /// <summary>
    /// Get media items by Ids
    /// </summary>
    Task<Dictionary<string, IMedia>> GetByIds(Ref[] references);

    /// <summary>
    /// Get source for a media item
    /// </summary>
    Task<string> GetUrl(Ref reference, bool isAbsolute = false);

    /// <summary>
    /// Get source for media items
    /// </summary>
    Task<Dictionary<string, string>> GetUrls(Ref[] references, bool isAbsolute = false);
  }
}
