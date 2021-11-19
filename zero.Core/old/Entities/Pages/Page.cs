using System;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <summary>
  /// A page can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  [Collection("Pages")]
  public class Page : ZeroEntity
  {
    /// <summary>
    /// Use this field (when filled out) instead of the alias for URL generation
    /// </summary>
    public string UrlAlias { get; set; }

    /// <inheritdoc />
    public string ParentId { get; set; }

    /// <summary>
    /// Alias of the used page type
    /// </summary>
    public string PageTypeAlias { get; set; }

    /// <summary>
    /// Date when the page is published
    /// </summary>
    public DateTimeOffset? PublishDate { get; set; }

    /// <summary>
    /// Date when the page is unpublished
    /// </summary>
    public DateTimeOffset? UnpublishDate { get; set; }
  }
}