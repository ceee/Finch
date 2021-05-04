using zero.Core.Attributes;

namespace zero.Core.Entities
{
  [Collection("Previews")]
  public class Preview : ZeroEntity
  {
    /// <summary>
    /// Id of the original entity
    /// </summary>
    public string OriginalId { get; set; }

    /// <summary>
    /// Contains the entity content
    /// </summary>
    public ZeroEntity Content { get; set; }
  }
}
