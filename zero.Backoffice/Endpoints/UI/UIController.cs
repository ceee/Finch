using Microsoft.AspNetCore.Mvc;
using System.Collections;
using zero.Backoffice.Services;

namespace zero.Backoffice.Endpoints.UI;

public class UIController : ZeroBackofficeController
{
  readonly IEnumerable<ISettingsGroup> SettingsGroups;
  readonly IAuthenticationService AuthenticationService;
  readonly IAuthorizationService AuthorizationService;
  readonly IApplicationStore ApplicationStore;
  readonly IIconService IconService;
  readonly IResourceService ResourceService;
  readonly ISectionService SectionService;

  public UIController(IEnumerable<ISettingsGroup> settingsGroups, 
    IAuthenticationService authenticationService, IAuthorizationService authorizationService, IApplicationStore applicationStore, IIconService iconService, IResourceService resourceService, ISectionService sectionService)
  {
    SettingsGroups = settingsGroups;
    AuthenticationService = authenticationService;
    AuthorizationService = authorizationService; 
    ApplicationStore = applicationStore;
    IconService = iconService;
    ResourceService = resourceService;
    SectionService = sectionService;
  }

  [HttpGet("sections")]
  //[ZeroAuthorize(CountryPermissions.Create)]
  public async Task<ActionResult<IEnumerable>> GetSections() => Ok(await SectionService.GetSections());


  [HttpGet("settingareas")]
  public async Task<ActionResult<IEnumerable>> GetSettingGroups() => Ok(await SectionService.GetSettingsAreas());


  [HttpGet("iconsets")]
  public async Task<ActionResult<IEnumerable>> GetIconSets()
  {
    return Ok(await IconService.GetSets());
  }


  [HttpGet("translations")]
  public async Task<ActionResult<Dictionary<string, string>>> GetTranslations()
  {
    return Ok(await ResourceService.GetTranslations("en-us"));
  }
}