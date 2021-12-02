namespace zero.Pages;

/// <summary>
/// A page can consist of unlimited properties and be rendered as you wish
/// The backoffice rendering is done by an IRenderer
/// </summary>
[RavenCollection("Pages")]
public class Page : ZeroEntity, ISupportsTrees
{
  /// <summary>
  /// Use this field (when filled out) instead of the alias for URL generation
  /// </summary>
  public string UrlAlias { get; set; }

  /// <inheritdoc />
  public string ParentId { get; set; }

  /// <summary>
  /// Date when the page is published
  /// </summary>
  public DateTimeOffset? PublishDate { get; set; }

  /// <summary>
  /// Date when the page is unpublished
  /// </summary>
  public DateTimeOffset? UnpublishDate { get; set; }
}