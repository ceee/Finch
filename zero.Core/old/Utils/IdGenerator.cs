using System;
using System.Linq;

namespace zero.Core.Utils
{
  public class IdGenerator
  {
    const string CHARS = "abcdefghijklmnopqrstuvwxyz0123456789";

    private static Random random = new();


    public static string Classic()
    {
      return Guid.NewGuid().ToString();
    }


    /// <summary>
    /// Create a new random Id
    /// </summary>
    public static string CreateRandom(int length)
    {
      return new string(Enumerable.Repeat(CHARS, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Create a new unique Id
    /// </summary>
    public static string Create(int length = -1)
    {
      if (length < 1)
      {
        length = 12;
      }

      return CreateRandom(length);

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
}
