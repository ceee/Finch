using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace zero.Core.Entities
{
  public class UserClaim
  {
    /// <summary>
    /// Gets or sets the claim type for this claim
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Convert to a claim
    /// </summary>
    public Claim ToClaim() => new Claim(Type, Value);

    public UserClaim() { }

    public UserClaim(string type, string value)
    {
      Type = type;
      Value = value;
    }

    public UserClaim(string type, string key, string value)
    {
      Type = type;
      Value = key + ":" + value;
    }

    public UserClaim(Claim claim)
    {
      Type = claim?.Type;
      Value = claim?.Value;
    }
  }



  public class UserClaimComparer : IEqualityComparer<UserClaim>
  {
    public bool Equals(UserClaim x, UserClaim y)
    {
      return (x == null && y == null) || (x.Type.Equals(y.Type, StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(y.Value, StringComparison.InvariantCultureIgnoreCase));
    }

    public int GetHashCode(UserClaim obj)
    {
      return (obj.Type + obj.Value).GetHashCode();
    }
  }
}
