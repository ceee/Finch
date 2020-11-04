using System.Collections.Generic;
using zero.Core.Attributes;

namespace zero.Core.Entities
{
  public class UserRole : ZeroEntity, IUserRole, IZeroDbConventions
  {
    /// <inheritdoc/>
    public string Description { get; set; }

    /// <inheritdoc/>
    public string Icon { get; set; }

    /// <inheritdoc/>
    public List<IUserClaim> Claims { get; set; } = new List<IUserClaim>();
  }


  [Collection("UserRoles")]
  public interface IUserRole : IZeroEntity, IAppAwareShareableEntity, IZeroDbConventions, IIdentityUserRole
  {
    /// <summary>
    /// Additional description
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// Displayed icon alongside name
    /// </summary>
    string Icon { get; set; }
  }
}