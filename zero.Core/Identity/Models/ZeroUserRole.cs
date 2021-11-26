namespace zero.Identity;

[RavenCollection("UserRoles")]
public class ZeroUserRole : ZeroIdentityRole
{
  /// <summary>
  /// Additional description
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// Displayed icon alongside name
  /// </summary>
  public string Icon { get; set; }
}