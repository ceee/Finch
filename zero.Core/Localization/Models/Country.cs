namespace zero.Localization;

[RavenCollection("Countries")]
public class Country : ZeroEntity
{
  /// <summary>
  /// Preferred countries are displayed on top in lists
  /// </summary>
  public bool IsPreferred { get; set; }

  /// <summary>
  /// Country code (ISO 3166-1)
  /// </summary>
  public string Code { get; set; }
}