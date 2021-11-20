namespace zero.Identity;

[RavenCollection("Users")]
public class BackofficeUser : ZeroIdentityUser
{
  /// <summary>
  /// Application the user registered in
  /// </summary>
  public string AppId { get; set; }

  /// <summary>
  /// Currently selected app id for the backoffice
  /// </summary>
  public string CurrentAppId { get; set; }

  /// <summary>
  /// Super user
  /// </summary>
  public bool IsSuper { get; set; }

  /// <summary>
  /// Avatar image
  /// </summary>
  public string AvatarId { get; set; }
}