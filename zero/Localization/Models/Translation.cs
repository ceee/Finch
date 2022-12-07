namespace zero.Localization;

public class Translation
{
  /// <summary>
  /// Key of the translation
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
  Html = 1
}