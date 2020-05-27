using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public class Language : ZeroEntity, ILanguage
  {
    /// <inheritdoc />
    public string Code { get; set; }

    /// <inheritdoc />
    public bool IsDefault { get; set; }

    /// <inheritdoc />
    public string InheritedLanguageId { get; set; }
  }

  [Collection("Languages")]
  public interface ILanguage : IZeroEntity, IZeroDbConventions
  {
    /// <summary>
    /// Language code (ISO 3166-1)
    /// </summary>
    string Code { get; set; }

    /// <summary>
    /// Whether this is the default language
    /// </summary>
    bool IsDefault { get; set; }

    /// <summary>
    /// If this language is inherited it gets all missing properties from its parent
    /// </summary>
    string InheritedLanguageId { get; set; }
  }
}