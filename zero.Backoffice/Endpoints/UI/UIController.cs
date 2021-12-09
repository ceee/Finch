using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.IO;
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
  readonly IMediaManagement Media;

  public UIController(IEnumerable<ISettingsGroup> settingsGroups, 
    IAuthenticationService authenticationService, IAuthorizationService authorizationService, IApplicationStore applicationStore, IIconService iconService, IResourceService resourceService, ISectionService sectionService, IMediaManagement media)
  {
    SettingsGroups = settingsGroups;
    AuthenticationService = authenticationService;
    AuthorizationService = authorizationService; 
    ApplicationStore = applicationStore;
    IconService = iconService;
    ResourceService = resourceService;
    SectionService = sectionService;
    Media = media;
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


  [HttpGet("mediapreview/{id}/{size?}")]
  public async Task<IActionResult> GetSource(string id, string size = null)
  {
    Media.Media media = await Media.GetFile(id);

    if (media == null)
    {
      return NotFound();
    }

    string path = Media.GetPublicFilePath(media);

    if (path == null)
    {
      return NotFound();
    }

    if (path.StartsWith("url://"))
    {
      path = path.Substring(6);
    }

    FileExtensionContentTypeProvider provider = new();
    string contentType;
    if (!provider.TryGetContentType(Path.GetFileName(path), out contentType))
    {
      contentType = "application/octet-stream";
    }

    try
    {
      return File(await Media.GetFileStream(media), contentType, DateTimeOffset.Now, EntityTagHeaderValue.Any);
    }
    catch (FileSystemException)
    {
      return NotFound();
    }
  }
}