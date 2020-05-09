namespace zero.Core.Entities
{
  /// <summary>
  /// A list item can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public class SpaceContent : ZeroEntity, IAppAwareEntity
  {
    /// <summary>
    /// Associated space
    /// </summary>
    public string SpaceAlias { get; set; }

    /// <inheritdoc />
    public string AppId { get; set; }
  }
}