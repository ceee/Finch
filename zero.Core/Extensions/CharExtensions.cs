using System.Collections.Generic;

namespace zero;

public static class CharExtensions
{
  private static Dictionary<char, char[]> accents { get; } = new Dictionary<char, char[]>()
  {
    { 'ä', new char[2] { 'a', 'e' } },
    { 'á', new char[1] { 'a' } },
    { 'à', new char[1] { 'a' } },
    { 'ó', new char[1] { 'o' } },
    { 'ò', new char[1] { 'o' } },
    { 'é', new char[1] { 'e' } },
    { 'è', new char[1] { 'e' } },
    { 'ú', new char[1] { 'u' } },
    { 'ù', new char[1] { 'u' } },
    { 'í', new char[1] { 'i' } },
    { 'ì', new char[1] { 'i' } },
    { 'ö', new char[2] { 'o', 'e' } },
    { 'ü', new char[2] { 'u', 'e' } },
    { 'ß', new char[2] { 's', 's' } },
    { '&', new char[1] { '+' } }
  };


  /// <summary>
  /// Check if a character is from a-z, A-Z or 0-9
  /// </summary>
  public static bool IsAZor09(this char value)
  {
    return (value >= 0x41 && value <= 0x5A) || (value >= 0x61 && value <= 0x7a) || (value >= 0x30 && value <= 0x39);
  }


  /// <summary>
  /// Check if a character is in ASCII range
  /// </summary>
  public static bool IsASCII(this char value)
  {
    return value < 128;
  }


  /// <summary>
  /// Replaces an accent or umlaut with the appropriate URL + file ready variant
  /// </summary>
  public static bool TryReplaceAccent(this char value, out char[] result)
  {
    if (!accents.ContainsKey(value))
    {
      result = null;
      return false;
    }

    result = accents[value];
    return true;
  }
}
