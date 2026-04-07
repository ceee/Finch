using System.Security.Claims;

namespace Finch.Identity;

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
  public Claim ToClaim() => new(Type, Value);

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