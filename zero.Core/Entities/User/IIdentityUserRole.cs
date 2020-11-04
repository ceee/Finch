using System.Collections.Generic;

namespace zero.Core.Entities
{
  public interface IIdentityUserRole : IAppAwareEntity, IZeroEntity
  {
    /// <summary>
    /// The user's claims, for use in claims-based authentication.
    /// </summary>
    List<IUserClaim> Claims { get; set; }
  }
}
