using Microsoft.AspNetCore.Mvc;
using System.Collections;
using zero.Api.Endpoints.Applications;

namespace zero.Backoffice.Endpoints.UI;

public class UIController : ZeroBackofficeController
{
  readonly IEnumerable<IBackofficeSection> Sections;
  readonly IEnumerable<ISettingsGroup> SettingsGroups;
  readonly IAuthenticationService AuthenticationService;
  readonly IAuthorizationService AuthorizationService;
  readonly IApplicationStore ApplicationStore;

  public UIController(IEnumerable<IBackofficeSection> sections, IEnumerable<ISettingsGroup> settingsGroups, 
    IAuthenticationService authenticationService, IAuthorizationService authorizationService, IApplicationStore applicationStore)
  {
    Sections = sections;
    SettingsGroups = settingsGroups;
    AuthenticationService = authenticationService;
    AuthorizationService = authorizationService; 
    ApplicationStore = applicationStore;
  }

  [HttpGet("sections")]
  //[ZeroAuthorize(CountryPermissions.Create)]
  public ActionResult<IEnumerable> GetSections() => Ok(Sections);


  [HttpGet("sections/settings")]
  public async Task<dynamic> GetAreas()
  {
    bool isSuperUser = AuthenticationService.IsSuper();
    IList<Permission> permissions = AuthorizationService.GetPermissions(Permissions.Settings.PREFIX);

    List<ZeroVueSettingsGroup> groups = new();

    foreach (ISettingsGroup group in SettingsGroups)
    {
      List<ZeroVueSettingsArea> areas = new();

      foreach (SettingsArea area in group.Areas)
      {
        //if (!isSuperUser && !Permission.CanReadKey(permissions, area.Alias, true))
        //{
        //  continue;
        //}

        ZeroVueSettingsArea vueArea = new()
        {
          Alias = area.Alias,
          Name = area.Name,
          Description = area.Description,
          Icon = area.Icon,
          Url = area.CustomUrl.Or(Constants.Sections.Settings.EnsureStartsWith('/') + Safenames.Alias(area.Alias).EnsureStartsWith('/'))
        };

        areas.Add(vueArea);
      }

      if (areas.Count > 0)
      {
        groups.Add(new()
        {
          Name = group.Name,
          Items = areas
        });
      }
    }

    List<Application> applications = await ApplicationStore.LoadAll();// new List<Application>();

    //if (Permission.CanReadKey(permissions, Permissions.Settings.Applications, false))
    //{
    //  applications = await ApplicationsApi.GetAll();
    //}

    return new UIViewModel()
    {
      Groups = groups,
      Applications = applications.Select(x => Mapper.Map<Application, ApplicationBasic>(x)).ToList()
    };
  }
}