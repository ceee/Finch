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
  public class ZeroMediaHelper : IZeroMediaHelper
  {
    HttpContext HttpContext;

    IMediaApi MediaApi;


    public ZeroMediaHelper(IHttpContextAccessor httpContextAccessor, IMediaApi mediaApi)
    {
      HttpContext = httpContextAccessor.HttpContext;
      MediaApi = mediaApi;
    }


    /// <inheritdoc />
    public async Task<IMedia> GetById(string id)
    {
      if (id.IsNullOrEmpty())
      {
        return null;
      }
      return await MediaApi.GetById(id);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, IMedia>> GetByIds(string[] ids)
    {
      return await MediaApi.GetById(ids);
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(string id, bool isAbsolute = false)
    {
      IMedia media = await GetById(id);
      return media?.Source;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, string>> GetUrls(string[] ids, bool isAbsolute = false)
    {
      Dictionary<string, IMedia> medias = await GetByIds(ids);
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
  }
}
