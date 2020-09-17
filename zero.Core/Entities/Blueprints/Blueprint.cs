using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public class Blueprint : IBlueprint
  {
    /// <inheritdoc />
    public string Id { get; set; }

    /// <inheritdoc />
    public IZeroEntity Content { get; set; }
  }


  [Collection("Blueprints")]
  public interface IBlueprint : IZeroIdEntity, IZeroDbConventions
  {
    /// <summary>
    /// Contains the entity content
    /// </summary>
    IZeroEntity Content { get; set; }
  }
}
