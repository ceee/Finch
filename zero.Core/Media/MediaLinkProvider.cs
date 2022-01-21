using Raven.Client.Documents.Session;

namespace zero.Media;

public class MediaLinkProvider : ILinkProvider
{
  public const string AREA = "zero.media";

  protected IRoutes Routes { get; set; }
  protected IMediaStore Media { get; set; }
  protected IMediaManagement MediaManagement { get; set; }

  public MediaLinkProvider(IRoutes routes, IMediaStore media, IMediaManagement mediaManagement)
  {
    Routes = routes;
    Media = media;
    MediaManagement = mediaManagement;
  }


  /// <summary>
  /// Creates a new link object from a media item
  /// </summary>
  public Link Create(Media media, LinkTarget target = LinkTarget.Default, string title = null)
  {
    return new Link()
    {
      Area = AREA,
      Target = target,
      Title = title,
      Values = new()
      {
        { "id", media.Id }
      }
    };
  }


  /// <inheritdoc />
  public bool CanProcess(Link link) => link.Area == AREA;


  /// <inheritdoc />
  public async Task<string> Resolve(Link link)
  {
    Media media = await Media.Load(link.Values.GetValueOrDefault("id"));

    if (media == null)
    {
      return null;
    }

    link.IsActive = true;
    link.Url = MediaManagement.GetPublicFilePath(media);
    return link.Url;
  }


  /// <inheritdoc />
  public async Task<LinkPreview> Preview(IAsyncDocumentSession session, Link link)
  {
    string id = link.Values.GetValueOrDefault("id");

    if (id.IsNullOrEmpty())
    {
      return null;
    }

    Media media = await session.LoadAsync<Media>(id);

    if (media == null)
    {
      return null;
    }

    string url = MediaManagement.GetPublicFilePath(media);

    return new()
    {
      Id = media.Id,
      Icon = media.IsFolder ? "fth-folder" : (media.ImageMeta != null ? "fth-image" : "fth-file"),
      Name = media.Name,
      IsActive = media.IsActive,
      Text = url
    };
  }
}
