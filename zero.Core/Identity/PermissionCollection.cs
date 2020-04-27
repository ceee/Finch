using System.Collections.Generic;

namespace zero.Core.Identity
{
  public class PermissionCollection
  {
    /// <summary>
    /// Name of the group
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Optional description text
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Individual permissions (stored as claims on the user identity)
    /// </summary>
    public IList<Permission> Items { get; set; } = new List<Permission>();
  }
}
