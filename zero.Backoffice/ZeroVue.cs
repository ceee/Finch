using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace zero.Backoffice;

public class ZeroVue : IZeroVue
{
  protected IZeroOptions Options { get; private set; }

  protected IPaths Paths { get; private set; }

  protected IApplicationStore ApplicationStore { get; private set; }

  protected IAuthenticationService AuthenticationApi { get; private set; }

  protected IEnumerable<IZeroPlugin> Plugins { get; private set; }

  protected IEnumerable<IBackofficeSection> Sections { get; private set; }

  protected IEnumerable<ISettingsGroup> SettingsGroups { get; private set; }

  protected IZeroContext Context { get; private set; }

  protected ILogger<IZeroVue> Logger { get; private set; }

  protected IZeroStore Store { get; private set; }


  public ZeroVue(IZeroOptions options, IPaths paths, IApplicationStore applicationStore, IAuthenticationService authenticationApi, 
    IEnumerable<IZeroPlugin> plugins, IZeroContext context, ILogger<IZeroVue> logger, IZeroStore store,
    IEnumerable<IBackofficeSection> sections, IEnumerable<ISettingsGroup> settingsGroups)
  {
    Paths = paths;
    Options = options;
    ApplicationStore = applicationStore;
    AuthenticationApi = authenticationApi;
    Plugins = plugins;
    Context = context;
    Logger = logger;
    Store = store;
    Sections = sections;
    SettingsGroups = settingsGroups;
  }


  /// <inheritdoc/>
  public async Task<ZeroVueConfig> Config()
  {
    ZeroVueConfig config = new();

    config.Path = Options.ZeroPath.EnsureEndsWith('/');
    config.ApiPath = config.Path + "api/";
    config.PluginPath = "@/Plugins";
    config.Version = Options.Version;
    config.PluginCount = Plugins.Count();
    config.ErrorFieldNone = Constants.ErrorFieldNone;
    config.Alias = CreateAliases();
    config.AppId = Context.AppId;
    //config.SharedAppId = Constants.Database.SharedAppId; // TODO appx
    config.MultiApps = Options.Applications.Count > 1;

    ZeroUser user = await AuthenticationApi.GetUser();

    if (user != null)
    {
      config.User = new
      {
        Id = user.Id,
        AvatarId = user.AvatarId,
        Email = user.Email,
        IsSuper = user.IsSuper,
        Name = user.Name,
        Roles = user.RoleIds
      };

      config.Applications = await CreateApplications();

      config.Plugins = Plugins.Select(x => new ZeroVuePlugin()
      {
        Name = x.Options.Name,
        Description = x.Options.Description,
        PluginPath = x.Options.PluginPath
      }).ToList();

      config.Services = new()
      {
        YouTubeApiKey = Options.For<ExternalServicesOptions>().YouTubeApiKey
      };

      config.Blueprints = CreateBlueprints();
    }

    return config;
  }


  /// <inheritdoc/>
  public async Task<string> ConfigAsJson()
  {
    return JsonSerializer.Serialize(await Config(), new JsonSerializerOptions()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
  }


  /// <summary>
  /// Create aliases
  /// </summary>
  Dictionary<string, Dictionary<string, string>> CreateAliases()
  {
    Dictionary<string, Dictionary<string, string>> aliases = new Dictionary<string, Dictionary<string, string>>();

    Dictionary<string, string> sections = new Dictionary<string, string>();
    sections.Add("dashboard", Constants.Sections.Dashboard);
    sections.Add("pages", Constants.Sections.Pages);
    sections.Add("spaces", Constants.Sections.Spaces);
    sections.Add("media", Constants.Sections.Media);
    sections.Add("settings", Constants.Sections.Settings);

    Dictionary<string, string> settings = new Dictionary<string, string>();
    settings.Add("applications", Constants.Settings.Applications);
    settings.Add("countries", Constants.Settings.Countries);
    settings.Add("logging", Constants.Settings.Logging);
    settings.Add("languages", Constants.Settings.Languages);
    settings.Add("translations", Constants.Settings.Translations);
    settings.Add("mails", Constants.Settings.Mails);
    settings.Add("updates", Constants.Settings.Updates);
    settings.Add("users", Constants.Settings.Users);
    settings.Add("integrations", Constants.Settings.Integrations);

    aliases.Add("sections", sections);
    aliases.Add("settings", settings);
    aliases.Add("pages", new Dictionary<string, string>()
    {
      { "folder", Constants.Pages.FolderAlias }
    });

    return aliases;
  }


  /// <summary>
  /// Get all visible applications
  /// </summary>
  async Task<List<ZeroVueApplication>> CreateApplications()
  {
    IEnumerable<Application> applications = await ApplicationStore.LoadAll();

    string[] mediaIds = applications.Select(x => x.IconId).Where(x => x != null).ToArray();
    Dictionary<string, Media.Media> media = await Store.Session().LoadAsync<Media.Media>(mediaIds);

    return applications.OrderBy(app => app.Sort).Select(app => new ZeroVueApplication()
    {
      Id = app.Id,
      Name = app.Name,
      Image = app.IconId.IsNullOrEmpty() ? null : media.GetValueOrDefault(app.IconId)?.ImageMeta?.Thumbnails.GetValueOrDefault("thumb")
    }).ToList();
  }


  List<ZeroVueBlueprint> CreateBlueprints()
  {
    List<ZeroVueBlueprint> items = new();

    if (!Options.For<BlueprintOptions>().Enabled)
    {
      return items;
    }

    foreach (Blueprint blueprint in Options.For<BlueprintOptions>())
    {
      string[] unlocked = blueprint.GetUnlockedFieldNames().ToArray();

      items.Add(new()
      {
        Alias = blueprint.Alias,
        Enabled = true,
        Unlocked = unlocked
      });
    }

    return items;
  }
}


public interface IZeroVue
{
  /// <summary>
  /// Creates the zero configuration for vue
  /// </summary>
  Task<ZeroVueConfig> Config();

  /// <summary>
  /// Creates the zero configuration for vue
  /// </summary>
  Task<string> ConfigAsJson();
}


public class ZeroVueConfig
{
  public string Path { get; set; }

  public string ApiPath { get; set; }

  public string PluginPath { get; set; }

  public IList<ZeroVuePlugin> Plugins { get; set; } = new List<ZeroVuePlugin>();

  public string Version { get; set; }

  public int PluginCount { get; set; }

  public string ErrorFieldNone { get; set; }

  public string AppId { get; set; }

  public bool MultiApps { get; set; }

  public dynamic User { get; set; }

  public IList<ZeroVueApplication> Applications { get; set; } = new List<ZeroVueApplication>();

  public Dictionary<string, Dictionary<string, string>> Alias { get; set; } = new Dictionary<string, Dictionary<string, string>>();

  public Dictionary<string, object> Overrides { get; set; } = new Dictionary<string, object>();

  public ZeroVueServices Services { get; set; } = new();

  public IList<ZeroVueBlueprint> Blueprints { get; set; } = new List<ZeroVueBlueprint>();
}


public class ZeroVuePlugin
{
  public string Name { get; set; }

  public string Description { get; set; }

  public string PluginPath { get; set; }
}

public class ZeroVueApplication
{
  public string Id { get; set; }

  public string Name { get; set; }

  public string Image { get; set; }
}

public class ZeroVueServices
{
  public string YouTubeApiKey { get; set; }
}

public class ZeroVueBlueprint
{
  public string Alias { get; set; }

  public bool Enabled { get; set; }

  public string[] Unlocked { get; set; } = Array.Empty<string>();
}