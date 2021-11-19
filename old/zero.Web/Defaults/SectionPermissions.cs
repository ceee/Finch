using zero.Core;
using zero.Core.Identity;

namespace zero.Web.Defaults
{
  public class SectionPermissions : PermissionCollection
  {
    public SectionPermissions()
    {
      Alias = Constants.PermissionCollections.Sections;
      Label = "@permission.collections.sections";
      Description = "@permission.collections.sections_description";

      Items.Add(new Permission(Permissions.Sections.Dashboard, "@sections.item.dashboard", null, PermissionValueType.Boolean));
      Items.Add(new Permission(Permissions.Sections.Pages, "@sections.item.pages", null, PermissionValueType.Boolean));
      Items.Add(new Permission(Permissions.Sections.Spaces, "@sections.item.spaces", null, PermissionValueType.Boolean));
      Items.Add(new Permission(Permissions.Sections.Media, "@sections.item.media", null, PermissionValueType.Boolean));
      Items.Add(new Permission(Permissions.Sections.Settings, "@sections.item.settings", null, PermissionValueType.Boolean));
    }
  }
}
