namespace zero.Backoffice.Modules;

public class BackofficeSectionPermissions : PermissionProvider
{
  public BackofficeSectionPermissions() : base("@permission.collections.sections") { }

  static string Prefix = "sections.";

  public static readonly Permission Dashboard = new(Prefix + "dashboard", "@sections.item.dashboard");
  public static readonly Permission Pages   = new(Prefix + "pages", "@sections.item.pages");
  public static readonly Permission Spaces = new(Prefix + "spaces", "@sections.item.spaces");
  public static readonly Permission Media = new(Prefix + "media", "@sections.item.media");
  public static readonly Permission Settings = new(Prefix + "settings", "@sections.item.settings");


  /// <inheritdoc />
  public override IEnumerable<Permission> GetPermissions() => new[] { Dashboard, Pages, Spaces, Media, Settings };
}