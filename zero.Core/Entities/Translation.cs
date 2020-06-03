namespace zero.Core.Entities
{
  public class Translation : ZeroEntity, ITranslation
  {
    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public string LanguageId { get; set; }

    /// <inheritdoc />
    public string Key { get; set; }

    /// <inheritdoc />
    public string Value { get; set; }

    /// <inheritdoc />
    public TranslationDisplay Display { get; set; }
  }


  public enum TranslationDisplay
  {
    Text = 0,
    HTML = 1
  }


  public interface ITranslation : IZeroEntity, ILanguageAwareEntity, IAppAwareShareableEntity, IZeroDbConventions
  {
    /// <summary>
    /// Key which can be used to query translations
    /// </summary>
    string Key { get; set; }

    /// <summary>
    /// Value of the translation
    /// </summary>
    string Value { get; set; }

    /// <summary>
    /// Display + input type
    /// </summary>
    TranslationDisplay Display { get; set; }
  }
}