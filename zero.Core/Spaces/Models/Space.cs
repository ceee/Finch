namespace zero.Spaces;

/// <summary>
/// 
/// </summary>
[RavenCollection("Spaces")]
public class Space : ZeroEntity
{
  /// <summary>
  /// Associated space
  /// </summary>
  public string SpaceTypeAlias { get; set; }
}