namespace zero.Core.Entities
{
  public class Translation : ZeroEntity, ILanguageAwareEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc />
    public string LanguageId { get; set; }

    /// <summary>
    /// Key which can be used to query translations
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Value of the translation
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Display + input type
    /// </summary>
    public TranslationDisplay Display { get; set; }
  }


  public enum TranslationDisplay
  {
    Text = 0,
    HTML = 1
  }
}