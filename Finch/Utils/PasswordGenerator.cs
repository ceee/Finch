namespace Finch.Utils;

public class PasswordGenerator
{
  const string CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-@#.:!?*";

  private static Random random = new();

  /// <summary>
  /// Create a new password
  /// </summary>
  public static string Random(int length = -1)
  {
    if (length < 1)
    {
      length = 12;
    }

    return new string(Enumerable.Repeat(CHARS, length).Select(s => s[random.Next(s.Length)]).ToArray());
  }
}
