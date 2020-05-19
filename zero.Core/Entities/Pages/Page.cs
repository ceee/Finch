namespace zero.Core.Entities
{
  /// <summary>
  /// A page can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public class Page : ZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Id of the parent page
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// Alias of the used page type
    /// </summary>
    public string PageTypeAlias { get; set; }

    /// <inheritdoc />
    public string AppId { get; set; }
  }
}