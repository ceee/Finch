namespace Finch.Identity;

public abstract class FinchIdentityRole : FinchEntity
{
  /// <summary>
  /// The role's claims, for use in claims-based authentication.
  /// </summary>
  public List<UserClaim> Claims { get; set; } = new();
}
