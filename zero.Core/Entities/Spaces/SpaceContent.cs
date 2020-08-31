using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <summary>
  /// A list item can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  public class SpaceContent : ZeroEntity, ISpaceContent
  {
    /// <inheritdoc />
    public string SpaceAlias { get; set; }

    /// <inheritdoc />
    public string AppId { get; set; }
  }


  /// <summary>
  /// A list item can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  [Collection("SpaceContents")]
  public interface ISpaceContent : IZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Associated space
    /// </summary>
    string SpaceAlias { get; set; }
  }
}