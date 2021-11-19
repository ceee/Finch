using System.Collections.Generic;

namespace zero;

public abstract class ZeroIdentityRole : ZeroEntity
{
  /// <summary>
  /// The role's claims, for use in claims-based authentication.
  /// </summary>
  public List<UserClaim> Claims { get; set; } = new();
}
