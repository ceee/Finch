using System.Collections.Generic;

namespace zero.Core.Entities
{
  public class UserRole : ZeroEntity, IUserRole
  {
    /// <inheritdoc />
    public string AppId { get; set; }

    /// <inheritdoc/>
    public string Description { get; set; }

    /// <inheritdoc/>
    public string Icon { get; set; }

    /// <inheritdoc/>
    public List<IUserClaim> Claims { get; set; } = new List<IUserClaim>();
  }


  public interface IUserRole : IZeroEntity, IAppAwareEntity, IZeroDbConventions
  {
    /// <summary>
    /// Additional description
    /// </summary>
    string Description { get; set; }

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