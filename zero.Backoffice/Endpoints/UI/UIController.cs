using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.IO;
using System.Reflection;
using zero.Backoffice.Services;

namespace zero.Backoffice.Endpoints.UI;

public class UIController : ZeroBackofficeController
{
  readonly IIconService IconService;
  readonly IResourceService ResourceService;
  readonly ISectionService SectionService;
  readonly IZeroOptions Options;
  readonly IMediaManagement Media;

  public UIController(IIconService iconService, IResourceService resourceService, ISectionService sectionService, IZeroOptions options, IMediaManagement media)
  {
    IconService = iconService;
    ResourceService = resourceService;
    SectionService = sectionService;
    Options = options;
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


  [HttpGet("thumbnail/{id}-{size}.tmp")]
  public async Task<IActionResult> GetThumbnail(string id, string size)
  {
    zero.Media.Media media = await Media.GetFile(id);

    if (media == null)
    {
      return Ok();
    }

    string path = Media.GetPublicFilePath(media, size);

    if (path == null)
    {
      return Ok();
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
      return File(await Media.GetFileStream(media, size), contentType, DateTimeOffset.Now, EntityTagHeaderValue.Any);
    }
    catch (FileSystemException)
    {
      return Ok();
    }
  }
}