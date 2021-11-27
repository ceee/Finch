namespace zero.Backoffice.UIComposition;

public class SectionPermissions : PermissionProvider
{
  public SectionPermissions() : base("@permissions.collections.sections") { }

  public const string Dashboard = "zero.sections.dashboard";
  public const string Pages = "zero.sections.pages";
  public const string Spaces = "zero.sections.spaces";
  public const string Media = "zero.sections.media";
  public const string Settings = "zero.sections.settings";


  /// <inheritdoc />
  public override IEnumerable<Permission> GetPermissions() => new Permission[] 
  { 
    new(Dashboard, "@sections.item.dashboard"),
    new(Pages, "@sections.item.pages"),
    new(Spaces, "@sections.item.spaces"),
    new(Media, "@sections.item.media"),
    new(Settings, "@sections.item.settings")
  };
}