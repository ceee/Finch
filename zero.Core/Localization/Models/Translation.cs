namespace zero.Localization;

[RavenCollection("Translations")]
public class Translation : ZeroEntity
{
  public Translation()
  {
    IsActive = true;
  }

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