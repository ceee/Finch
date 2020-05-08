namespace zero.Core.Entities
{
  public class Translation : DatabaseEntity, ILanguageAwareEntity
  {
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

    /// <inheritdoc />
    public string ParentEntityId { get; set; }


    public class Variant : LanguageVariant
    {
      public string Value { get; set; }
    }
  }


  public enum TranslationDisplay
  {
    Text = 0,
    HTML = 1
  }
}