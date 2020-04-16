using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace zero.Core.Entities
{
  public class UserClaim : IUserClaim
  {
    /// <inheritdoc/>
    public string Type { get; set; }

    /// <inheritdoc/>
    public string Value { get; set; }

    /// <inheritdoc/>
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



  public interface IUserClaim
  {
    /// <summary>
    /// Gets or sets the claim type for this claim
    /// </summary>
    string Type { get; set; }

    /// <summary>
    /// Gets or sets the claim value for this claim
    /// </summary>
    string Value { get; set; }

    /// <summary>
    /// Convert to a claim
    /// </summary>
    /// <returns></returns>
    Claim ToClaim();
  }


  public class UserClaimComparer : IEqualityComparer<IUserClaim>
  {
    public bool Equals(IUserClaim x, IUserClaim y)
    {
      return (x == null && y == null) || (x.Type.Equals(y.Type, StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(y.Value, StringComparison.InvariantCultureIgnoreCase));
    }

    public int GetHashCode(IUserClaim obj)
    {
      return (obj.Type + obj.Value).GetHashCode();
    }
  }
}
