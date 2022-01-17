using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace zero.Routing;

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
  public async Task<string> GetUrl(Link link)
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
  public async Task<Dictionary<Link, string>> GetUrls(params Link[] links)
  {
    Dictionary<Link, string> result = new();

    foreach (Link link in links)
    {
      result.Add(link, await GetUrl(link));
    }

    return result;
  }


  /// <inheritdoc />
  public async Task<Link> Resolve(Link link)
  {
    ILinkProvider provider = Providers.LastOrDefault(x => x.CanProcess(link));

    if (provider == null)
    {
      Logger.LogWarning("Could not find provider for link with area {area}", link.Area);
      return null;
    }

    await provider.Resolve(link);

    return link;
  }


  /// <inheritdoc />
  public async Task<Dictionary<Link, Link>> Resolve(params Link[] links)
  {
    Dictionary<Link, Link> result = new();

    foreach (Link link in links)
    {
      Link resolved = await Resolve(link);
      result.Add(link, resolved);
    }

    return result;
  }


  /// <inheritdoc />
  public ILinkProvider GetProvider(Link link)
  {
    return Providers.LastOrDefault(x => x.CanProcess(link));
  }


  /// <inheritdoc />
  public async Task<string> Parse(string html)
  {
    MatchCollection matches =  Regex.Matches(html, "zero-link=\"(.+?)\"", RegexOptions.IgnoreCase);

    foreach (Match match in matches)
    {
      if (!match.Success)
      {
        continue;
      }

      string valueJson = WebUtility.HtmlDecode(match.Groups[1].Value);
      LinkFromRTE link = JsonSerializer.Deserialize<LinkFromRTE>(valueJson, new JsonSerializerOptions()
      { 
        PropertyNameCaseInsensitive = true
      });

      string url = await GetUrl(link.ToLink());
      string href = url.HasValue() ? "href=\"" + url + "\"" : string.Empty;

      html = html.Replace(match.Value, href);
    }

    return html;
  }


  class LinkFromRTE
  {
    public string Area { get; set; }
    public string Suffix { get; set; }
    public Dictionary<string, string> Values { get; set; } = new();

    public Link ToLink() => new()
    {
      Area = Area,
      UrlSuffix = Suffix,
      Values = Values
    };
  }
}

public interface ILinks
{
  /// <summary>
  /// Get URL from a link object by finding a provider which can resolve the link
  /// </summary>
  Task<string> GetUrl(Link link);

  /// <summary>
  /// Get URLs from link objects by finding matching providers
  /// </summary>
  Task<Dictionary<Link, string>> GetUrls(params Link[] links);

  /// <summary>
  /// Get resolved Link from a link object by finding a provider which can resolve the link
  /// </summary>
  Task<Link> Resolve(Link link);

  /// <summary>
  /// Get resolved Link from link objects by finding matching providers
  /// </summary>
  Task<Dictionary<Link, Link>> Resolve(params Link[] links);

  /// <summary>
  /// Get the provider for a specific link
  /// </summary>
  ILinkProvider GetProvider(Link link);

  /// <summary>
  /// Find a provider by a specific type
  /// </summary>
  T GetProvider<T>() where T : class, ILinkProvider;

  /// <summary>
  /// Parses HTML which comes from the rich-text-editor and converts link references.
  /// </summary>
  Task<string> Parse(string html);
}
