namespace zero.Core.Entities
{
  public class Country : ZeroEntity, ILanguageAwareEntity
  {
    /// <summary>
    /// Language variant
    /// </summary>
    public class Variant : LanguageVariant { }

    /// <summary>
    /// Preferred countries are displayed on top in lists
    /// </summary>
    public bool IsPreferred { get; set; }

    /// <summary>
    /// Country code (ISO 3166-1)
    /// </summary>
    public string Code { get; set; }

    /// <inheritdoc />
    public string LanguageId { get; set; }
  }
}
