using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;
using zero.Backoffice.Services;

namespace zero.Backoffice.Endpoints.UI;

public class UIController : ZeroBackofficeController
{
  readonly IIconService IconService;
  readonly IResourceService ResourceService;
  readonly ISectionService SectionService;
  readonly IZeroOptions Options;

  public UIController(IIconService iconService, IResourceService resourceService, ISectionService sectionService, IZeroOptions options)
  {
    IconService = iconService;
    ResourceService = resourceService;
    SectionService = sectionService;
    Options = options;
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


  [HttpGet("flavors")]
  public ActionResult<Dictionary<string, FlavorProvider>> GetFlavors()
  {
    Dictionary<string, FlavorProvider> result = new();

    foreach ((Type type, FlavorProvider provider) in Options.For<FlavorOptions>().Providers)
    {
      string key = type.GetCustomAttribute<RavenCollectionAttribute>(true)?.Name ?? type.Name;
      result[Safenames.Alias(key)] = provider;
    }

    return result;
  }


  [HttpGet("blueprints")]
  public ActionResult<IEnumerable<string>> GetBlueprints()
  {
    HashSet<string> result = new();

    foreach (Blueprint blueprint in Options.For<BlueprintOptions>())
    {
      string key = blueprint.ContentType.GetCustomAttribute<RavenCollectionAttribute>(true)?.Name ?? blueprint.Alias;
      result.Add(Safenames.Alias(key));
    }

    return result;
  }
}