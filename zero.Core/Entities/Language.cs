using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public class Language : ZeroEntity, ILanguage
  {
    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public string Code { get; set; }

    /// <inheritdoc />
    public bool IsDefault { get; set; }

    /// <inheritdoc />
    public bool IsOptional { get; set; }

    /// <inheritdoc />
    public string InheritedLanguageId { get; set; }
  }

  [Collection("Languages")]
  public interface ILanguage : IZeroEntity, IAppAwareShareableEntity, IZeroDbConventions
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
    /// Whether this language is optional and does not have to be filled out
    /// </summary>
    bool IsOptional { get; set; }

    /// <summary>
    /// If this language is inherited it gets all missing properties from its parent
    /// </summary>
    string InheritedLanguageId { get; set; }
  }
}