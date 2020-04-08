using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class UserRole : DatabaseEntity, IUserRole
  {
    /// <inheritdoc/>
    public string Icon { get; set; }

    /// <inheritdoc/>
    public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
  }


  public interface IUserRole : IDatabaseEntity
  {
    /// <summary>
    /// Displayed icon alongside name
    /// </summary>
    string Icon { get; set; }

    /// <summary>
    /// The user's claims, for use in claims-based authentication.
    /// </summary>
    List<UserClaim> Claims { get; set; }
  }
}