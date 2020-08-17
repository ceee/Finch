using System;

namespace zero.Core.Entities
{
  /// <summary>
  /// A page can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public class Page : ZeroEntity, IPage
  {
    /// <inheritdoc />
    public string ParentId { get; set; }

    /// <inheritdoc />
    public string PageTypeAlias { get; set; }

    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public bool IsRecycled { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? PublishDate { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UnpublishDate { get; set; }
  }


  public interface IPage : IZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Id of the parent page
    /// </summary>
    string ParentId { get; set; }

    /// <summary>
    /// Alias of the used page type
    /// </summary>
    string PageTypeAlias { get; set; }

    /// <summary>
    /// Whether this page is recycled or not
    /// </summary>
    bool IsRecycled { get; set; }

    /// <summary>
    /// Date when the page is published
    /// </summary>
    DateTimeOffset? PublishDate { get; set; }

    /// <summary>
    /// Date when the page is unpublished
    /// </summary>
    DateTimeOffset? UnpublishDate { get; set; }
  }
}