using System.Security.Claims;

namespace zero.Core.Entities
{
  public class BackofficeUserClaim : IBackofficeUserClaim
  {
    /// <inheritdoc/>
    public string Type { get; set; }

    /// <inheritdoc/>
    public string Value { get; set; }

    /// <inheritdoc/>
    public Claim ToClaim() => new Claim(Type, Value);

    /// <inheritdoc/>
    public BackofficeUserClaim FromClaim(Claim other)
    {
      Type = other?.Type;
      Value = other?.Value;
      return this;
    }
  }



  public interface IBackofficeUserClaim
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
    /// Constructs a new claim with the type and value
    /// </summary>
    BackofficeUserClaim FromClaim(Claim other);

    /// <summary>
    /// Initializes by copying ClaimType and ClaimValue from the other claim
    /// </summary>
    Claim ToClaim();
  }
}
