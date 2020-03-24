using System.Collections.Generic;

namespace unjo.Core.Entities
{
  public class BackofficeUserRole : DatabaseEntity, IBackofficeUserRole
  {
    /// <inheritdoc/>
    public string Alias { get; set; }

    /// <inheritdoc/>
    public string Icon { get; set; }

    /// <inheritdoc/>
    public List<IBackofficeUserClaim> Claims { get; set; } = new List<IBackofficeUserClaim>();
  }


  public interface IBackofficeUserRole : IDatabaseEntity
  {
    /// <summary>
    /// Alias for natural queries
    /// </summary>
    string Alias { get; set; }

    /// <summary>
    /// Displayed icon alongside name
    /// </summary>
    string Icon { get; set; }

    /// <summary>
    /// The user's claims, for use in claims-based authentication.
    /// </summary>
    List<IBackofficeUserClaim> Claims { get; set; }
  }
}
