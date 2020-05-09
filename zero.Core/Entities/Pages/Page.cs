namespace zero.Core.Entities
{
  /// <summary>
  /// A page can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public abstract class Page : ZeroEntity, IAppAwareEntity
  {
    /// <inheritdoc />
    public string AppId { get; set; }
  }
}
