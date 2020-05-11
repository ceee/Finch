namespace zero.Core.Entities
{
  public class Language : ZeroEntity, IZeroDbConventions
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
    /// If this language is inherited it gets all missing properties from its parent
    /// </summary>
    public string InheritedLanguageId { get; set; }
  }
}