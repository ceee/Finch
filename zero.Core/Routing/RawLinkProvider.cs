using Raven.Client.Documents.Session;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Routing
{
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
    public bool CanProcess(Link link) => link.Area == AREA;


    /// <inheritdoc />
    public Task<string> Resolve(Link link)
    {
      link.Url = link.Values.GetValueOrDefault<string>("url");
      return Task.FromResult(link.Url);
    }


    /// <inheritdoc />
    public Task<PreviewModel> Preview(IAsyncDocumentSession session, Link link)
    {
      string url = link.Values.GetValueOrDefault<string>("url");

      if (url.IsNullOrEmpty())
      {
        return Task.FromResult<PreviewModel>(default);
      }

      return Task.FromResult(new PreviewModel()
      {
        Id = url,
        Icon = "fth-link",
        Name = link.Title.Or("@ui.link"),
        Text = url
      });
    }
  }
}
