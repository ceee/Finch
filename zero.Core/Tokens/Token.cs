using zero.Core.Attributes;
using zero.Core.Entities;

namespace zero.Core.Tokens
{
  [Collection("Tokens")]
  public class SecurityToken : ZeroEntity, IZeroDbConventions
  {
    public byte[] Key { get; set; }

    public string Token { get; set; }

    public object Value { get; set; }
  }
}
