using zero.Core;
using zero.Core.Identity;

namespace zero.Web.Defaults
{
  public class SpacePermissions : PermissionCollection
  {
    public SpacePermissions()
    {
      Alias = Constants.PermissionCollections.Spaces;
      Label = "@permission.collections.spaces";
      Description = "@permission.collections.spaces_description";

      // Items get added at runtime
    }
  }
}
