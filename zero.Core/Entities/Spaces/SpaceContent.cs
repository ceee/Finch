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
  }


  /// <summary>
  /// A list item can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  [Collection("SpaceContents", LongId = true)]
  public interface ISpaceContent : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Associated space
    /// </summary>
    string SpaceAlias { get; set; }
  }
}