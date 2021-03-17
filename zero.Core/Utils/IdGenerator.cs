using System;

namespace zero.Core.Utils
{
  public class IdGenerator
  {
    /// <summary>
    /// Create a new unique Id
    /// </summary>
    public static string Create(int length = -1)
    {
      if (length < 1)
      {
        length = 12;
      }
      //if (length > 0)
      //{
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
          .Replace("/", String.Empty)
          .Replace("+", String.Empty)
          .Replace("-", String.Empty)
          .ToLowerInvariant()
          .Substring(0, length);
      //}

      //return Guid.NewGuid().ToString();
    }
  }
}
