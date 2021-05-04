using zero.Core.Attributes;

namespace zero.Core.Entities
{
  /// <summary>
  /// A list item can consist of unlimited properties and be rendered as you wish
  /// The backoffice rendering is done by an IRenderer
  /// </summary>
  [Collection("SpaceContents")]
  public class SpaceContent : ZeroEntity
  {
    /// <summary>
    /// Associated space
    /// </summary>
    public string SpaceAlias { get; set; }
  }
}