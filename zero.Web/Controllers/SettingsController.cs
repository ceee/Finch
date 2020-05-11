using zero.Core.Api;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Settings, PermissionsValue.Read)]
  public class SettingsController : BackofficeController
  {
    ISettingsApi Api;

    public SettingsController(ISettingsApi api)
    {
      Api = api;
    }
  }
}
