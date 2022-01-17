using Raven.Client.Documents.Session;

namespace zero.Routing;

public class RawLinkProvider : ILinkProvider
{
  public const string AREA = "zero.url";


  /// <summary>
  /// Creates a new link object from an url
  /// </summary>
  public Link Create(string url, LinkTarget target = LinkTarget.Default, string title = null)
  {
    return new Link()
    {
      Area = AREA,
      Target = target,
      Title = title,
      Values = new()
      {
        { "url", url }
      }
    };
  }


  /// <inheritdoc />
  public bool CanProcess(Link link) => link.Area is AREA or "zero.raw";


  /// <inheritdoc />
  public Task<string> Resolve(Link link)
  {
    link.Url = link.Values.GetValueOrDefault("url");
    link.IsActive = true;
    return Task.FromResult(link.Url);
  }


  /// <inheritdoc />
  public Task<LinkPreview> Preview(IAsyncDocumentSession session, Link link)
  {
    string url = link.Values.GetValueOrDefault("url");

    if (url.IsNullOrEmpty())
    {
      return Task.FromResult<LinkPreview>(default);
    }

    return Task.FromResult(new LinkPreview()
    {
      Id = url,
      Icon = "fth-link",
      Name = link.Title.Or("@ui.link"),
      IsActive = true,
      Text = url
    }); ;
  }
}
