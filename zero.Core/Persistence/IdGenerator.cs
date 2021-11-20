namespace zero.Persistence;

public class IdGenerator
{
  const string CHARS = "abcdefghijklmnopqrstuvwxyz0123456789";

  private static Random random = new();

  /// <summary>
  /// Create a new unique Id
  /// </summary>
  public static string Create(int length = -1)
  {
    if (length < 1)
    {
      length = 12;
    }

    return new string(Enumerable.Repeat(CHARS, length).Select(s => s[random.Next(s.Length)]).ToArray());

    //if (length > 0)
    //{
    //return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
    //  .Replace("/", String.Empty)
    //  .Replace("+", String.Empty)
    //  .Replace("-", String.Empty)
    //  .ToLowerInvariant()
    //  .Substring(0, length);
    //}

    //return Guid.NewGuid().ToString();
  }
}
