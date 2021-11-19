using System;
using System.Text.RegularExpressions;
using zero.Core.Options;

namespace zero.Core.Services
{
  public class GuidCreator : IGuidCreator
  {
    protected Regex InvalidCharacters = new Regex("[/+-]", RegexOptions.Compiled);

    protected IZeroOptions Options { get; set; }


    public GuidCreator(IZeroOptions options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public string New(int length = -1)
    {
      return Generate(length > 0 ? length : 16);
    }


    /// <inheritdoc />
    public double CollisionChance(int length)
    {
      return Math.Sqrt(Math.Pow(36, length));
    }


    protected virtual string Generate(int length)
    {
      return InvalidCharacters.Replace(GenerateBaseString(), String.Empty)
          .ToLowerInvariant()
          .Substring(0, length);
    }


    protected virtual string GenerateBaseString()
    {
      return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
  }

  public interface IGuidCreator
  {
    /// <summary>
    /// Creates a new ID based on a GUID with the specified length or the default length
    /// </summary>
    string New(int length = -1);

    /// <summary>
    /// Counts the number of occurrences which approximately results in a collision probability of 50%
    /// </summary>
    double CollisionChance(int length);
  }
}
