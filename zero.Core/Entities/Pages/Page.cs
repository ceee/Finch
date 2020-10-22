using Microsoft.AspNetCore.Mvc;
using System;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <summary>
  /// A page can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public class Page : ZeroEntity, IPage
  {
    /// <inheritdoc />
    public UrlRoute Route { get; set; }

    /// <inheritdoc />
    public string ParentId { get; set; }

    /// <inheritdoc />
    public string PageTypeAlias { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? PublishDate { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? UnpublishDate { get; set; }
  }


  [Collection("Pages")]
  public interface IPage : IZeroEntity, IAppAwareEntity, IZeroDbConventions, IRoutedEntity
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
    /// Date when the page is published
    /// </summary>
    DateTimeOffset? PublishDate { get; set; }

    /// <summary>
    /// Date when the page is unpublished
    /// </summary>
    DateTimeOffset? UnpublishDate { get; set; }
  }
}