using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Tokens
{
  [Collection("Tokens")]
  public class SecurityToken : IZeroDbConventions
  {
    public string Id { get; set; }

    public string Key { get; set; }

    public string Token { get; set; }
  }
}
