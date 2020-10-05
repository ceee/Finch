using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public class Preview : ZeroEntity, IPreview
  {
    /// <inheritdoc />
    public string OriginalId { get; set; }

    /// <inheritdoc />
    public IZeroEntity Content { get; set; }
  }


  [Collection("Previews")]
  public interface IPreview : IZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Id of the original entity
    /// </summary>
    string OriginalId { get; set; }

    /// <summary>
    /// Contains the entity content
    /// </summary>
    IZeroEntity Content { get; set; }
  }
}
