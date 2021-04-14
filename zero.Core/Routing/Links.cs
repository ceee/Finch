using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;

namespace zero.Core.Routing
{
  public class Links : ILinks
  {
    protected IZeroStore Store { get; set; }
    protected ILogger<Links> Logger { get; set; }
    protected IEnumerable<ILinkProvider> Providers { get; set; }

    public Links(IZeroStore store, ILogger<Links> logger, IEnumerable<ILinkProvider> providers)
    {
      Store = store;
      Logger = logger;
      Providers = providers;
    }


    /// <inheritdoc />
    public T GetProvider<T>() where T : class, ILinkProvider
    {
      Type type = typeof(T);
      return Providers.FirstOrDefault(x => x.GetType().IsAssignableFrom(type)) as T;
    }


    /// <inheritdoc />
    public async Task<string> GetUrl(ILink link)
    {
      ILinkProvider provider = Providers.LastOrDefault(x => x.CanProcess(link));

      if (provider == null)
      {
        Logger.LogWarning("Could not find provider for link with area {area}", link.Area);
        return null;
      }

      return await provider.Resolve(link);
    }


    /// <inheritdoc />
    public async Task<Dictionary<ILink, string>> GetUrls(params ILink[] links)
    {
      Dictionary<ILink, string> result = new();

      foreach (ILink link in links)
      {
        result.Add(link, await GetUrl(link));
      }

      return result;
    }


    /// <inheritdoc />
    public ILinkProvider GetProvider(ILink link)
    {
      return Providers.LastOrDefault(x => x.CanProcess(link));
    }
  }

  public interface ILinks
  {
    /// <summary>
    /// Get URL from a link object by finding a provider which can resolve the link
    /// </summary>
    Task<string> GetUrl(ILink link);

    /// <summary>
    /// Get URLs from link objects by finding matching providers
    /// </summary>
    Task<Dictionary<ILink, string>> GetUrls(params ILink[] links);

    /// <summary>
    /// Get the provider for a specific link
    /// </summary>
    ILinkProvider GetProvider(ILink link);

    /// <summary>
    /// Find a provider by a specific type
    /// </summary>
    T GetProvider<T>() where T : class, ILinkProvider;
  }
}
