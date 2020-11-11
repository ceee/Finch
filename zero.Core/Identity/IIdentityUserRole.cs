using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public interface IIdentityUserRole : IZeroEntity
  {
    /// <summary>
    /// The user's claims, for use in claims-based authentication.
    /// </summary>
    List<IUserClaim> Claims { get; set; }
  }
}
