namespace zero.Localization;

[RavenCollection("Languages")]
public class Language : ZeroEntity, IAlwaysActive
{
  /// <summary>
  /// Language code (ISO 3166-1)
  /// </summary>
  public string Code { get; set; }

  /// <summary>
  /// Whether this is the default language
  /// </summary>
  public bool IsDefault { get; set; }

  /// <summary>
  /// Whether this language is optional and does not have to be filled out
  /// </summary>
  public bool IsOptional { get; set; }

  /// <summary>
  /// If this language is inherited it gets all missing properties from its parent
  /// </summary>
  public string InheritedLanguageId { get; set; }
}