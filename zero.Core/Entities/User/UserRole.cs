using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class UserRole : DatabaseEntity, IUserRole
  {
    /// <inheritdoc/>
    public string Icon { get; set; }

    /// <inheritdoc/>
    public List<IUserClaim> Claims { get; set; } = new List<IUserClaim>();
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
    List<IUserClaim> Claims { get; set; }
  }
}