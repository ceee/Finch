using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public class RecycledEntity : ZeroEntity, IRecycledEntity
  {
    /// <inheritdoc />
    public string OriginalId { get; set; }

    /// <inheritdoc />
    public string OperationId { get; set; }

    /// <inheritdoc />
    public string Group { get; set; }

    /// <inheritdoc />
    public IZeroEntity Content { get; set; }
  }


  [Collection("RecycleBin")]
  public interface IRecycledEntity : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Id of the recycled entity
    /// </summary>
    string OriginalId { get; set; }

    /// <summary>
    /// Group recycled entities together which have been recycled in a single operation
    /// </summary>
    string OperationId { get; set; }

    /// <summary>
    /// Contains the entity content
    /// </summary>
    IZeroEntity Content { get; set; }

    /// <summary>
    /// Recycled entities can be grouped together (e.g. pages, media, ...)
    /// </summary>
    string Group { get; set; }
  }
}
