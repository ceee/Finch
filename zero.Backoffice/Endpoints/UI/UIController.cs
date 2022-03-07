using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Net.Http.Headers;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Reflection;
using zero.Backoffice.Services;
using zero.Preview;

namespace zero.Backoffice.Endpoints.UI;

public class UIController : ZeroBackofficeController
{
  readonly IIconService IconService;
  readonly IResourceService ResourceService;
  readonly ISectionService SectionService;
  readonly IZeroOptions Options;
  readonly IMediaManagement Media;
  readonly ICultureService CultureService;
  readonly IUserService UserService;
  readonly IRequestUrlResolver RequestUrlResolver;
  readonly IPreviewService PreviewService;
  readonly IZeroContext ZeroContext;
  readonly HttpClient HttpClient;

  public UIController(IUserService userService, IIconService iconService, IResourceService resourceService, ISectionService sectionService, IZeroOptions options, IMediaManagement media, 
    ICultureService cultureService, IRequestUrlResolver requestUrlResolver, IPreviewService previewService, IZeroContext zeroContext)
  {
    UserService = userService;
    IconService = iconService;
    ResourceService = resourceService;
    SectionService = sectionService;
    CultureService = cultureService;  
    Options = options;
    Media = media;
    RequestUrlResolver = requestUrlResolver;
    PreviewService = previewService;
    ZeroContext = zeroContext;
    HttpClient = new HttpClient();
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
  [ZeroAuthorize(false)]
  public async Task<ActionResult<Dictionary<string, string>>> GetTranslations()
  {
    ZeroUser user = await UserService.GetCurrentUser();
    string cultureCode = user?.LanguageId.Or("en-us");
    return Ok(await ResourceService.GetTranslations(cultureCode));
  }


  [HttpGet("cultures")]
  public ActionResult<List<Culture>> GetCultures()
  {
    return Ok(CultureService.GetAllCultures(Options.For<BackofficeOptions>().SupportedLanguages));
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


  [HttpPost("previews/token")]
  public async Task<ActionResult<PreviewRequestTokenModel.Response>> GetPreviewToken(PreviewRequestTokenModel model)
  {
    string token = await PreviewService.CreateAccessToken(model.Key, ZeroContext.BackofficeUser);
    return new PreviewRequestTokenModel.Response()
    {
      Token = token,
      QueryParameter = Options.For<PreviewOptions>().QueryParameter
    };
  }


  [HttpGet("thumbnail/{id}-{size}.tmp")]
  public async Task<IActionResult> GetThumbnail(string id, string size)
  {
    Stream stream = null;
    zero.Media.Media media = await Media.GetFile(id);

    if (media == null)
    {
      return NotFound();
    }

    try
    {
      stream = await Media.GetFileStream(media, size);
    }
    catch { }

    if (stream == null)
    {
      string fullPath = Media.GetPublicFilePath(media, size);

      if (RequestUrlResolver.IsAbsolute(fullPath))
      {
        byte[] bytes = await HttpClient.GetByteArrayAsync(fullPath);
        return File(bytes, "image/webp"); // TODO this is shit and should not be here in zero (only for CDN link)
      }

      return Ok();
    }

    try
    {
      return File(stream, "image/jpeg", DateTimeOffset.Now, EntityTagHeaderValue.Any); // TODO we do not query for the content-type
    }
    catch (FileSystemException)
    {
      return Ok();
    }
  }
}