using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Sections.Settings, PermissionsValue.Read)]
  public class SettingsController : BackofficeController
  {
    IAuthenticationApi AuthApi;

    IApplicationsApi ApplicationsApi;


    public SettingsController(IAuthenticationApi authApi, IApplicationsApi applicationsApi)
    {
      AuthApi = authApi;
      ApplicationsApi = applicationsApi;
    }



    /// <summary>
    /// Get all settings areas
    /// </summary>    
    public async Task<dynamic> GetAreas()
    {
      bool isSuperUser = AuthApi.IsSuper();
      IList<Permission> permissions = AuthApi.GetPermissions(Permissions.Settings.PREFIX);

      List<ZeroVueSettingsGroup> groups = new List<ZeroVueSettingsGroup>();

      foreach (SettingsGroup group in Options.Settings.GetAllItems())
      {
        List<ZeroVueSettingsArea> areas = new List<ZeroVueSettingsArea>();

        foreach (SettingsArea area in group.Items)
        {
          if (!isSuperUser && !Permission.CanReadKey(permissions, area.Alias, true))
          {
            continue;
          }

          ZeroVueSettingsArea vueArea = new ZeroVueSettingsArea()
          {
            Alias = area.Alias,
            Name = area.Name,
            Description = area.Description,
            Icon = area.Icon,
            Url = Constants.Sections.Settings.EnsureStartsWith('/') + Safenames.Alias(area.Alias).EnsureStartsWith('/')
          };

          areas.Add(vueArea);
        }

        if (areas.Count > 0)
        {
          groups.Add(new ZeroVueSettingsGroup()
          {
            Name = group.Name,
            Items = areas
          });
        }
      }

      IList<IApplication> applications = new List<IApplication>();

      if (Permission.CanReadKey(permissions, Permissions.Settings.Applications, false))
      {
        applications = await ApplicationsApi.GetAll();
      }

      return new
      {
        groups,
        applications
      };
    }
  }
}