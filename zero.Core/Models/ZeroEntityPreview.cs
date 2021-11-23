namespace zero.Models;

[RavenCollection("Previews")]
public class ZeroEntityPreview : ZeroEntity
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