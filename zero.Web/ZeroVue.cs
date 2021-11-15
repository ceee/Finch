using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using zero.Core;
using zero.Core.Api;
using zero.Core.Blueprints;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Web.Models;

namespace zero.Web
{
  public class ZeroVue : IZeroVue
  {
    protected IZeroOptions Options { get; private set; }

    protected IPaths Paths { get; private set; }

    protected IApplicationsApi ApplicationsApi { get; private set; }

    protected IAuthenticationApi AuthenticationApi { get; private set; }

    protected IEnumerable<IZeroPlugin> Plugins { get; private set; }

    protected IZeroContext Context { get; private set; }

    protected ILogger<IZeroVue> Logger { get; private set; }

    protected IZeroStore Store { get; private set; }

    string IconSymbolsSvg { get; set; }


    public ZeroVue(IZeroOptions options, IPaths paths, IApplicationsApi applicationsApi, IAuthenticationApi authenticationApi, IEnumerable<IZeroPlugin> plugins, IZeroContext context, ILogger<IZeroVue> logger, IZeroStore store)
    {
      Paths = paths;
      Options = options;
      ApplicationsApi = applicationsApi;
      AuthenticationApi = authenticationApi;
      Plugins = plugins;
      Context = context;
      Logger = logger;
      Store = store;
    }


    /// <inheritdoc/>
    public async Task<ZeroVueConfig> Config()
    {
      ZeroVueConfig config = new ZeroVueConfig();

      config.Path = Options.BackofficePath.EnsureEndsWith('/');
      config.ApiPath = config.Path + "api/";
      config.PluginPath = "@/Plugins";
      config.Version = Options.ZeroVersion;
      config.PluginCount = Plugins.Count();
      config.ErrorFieldNone = Constants.ErrorFieldNone;     
      config.Alias = CreateAliases();
      config.AppId = Context.AppId;
      //config.SharedAppId = Constants.Database.SharedAppId; // TODO appx
      config.Icons = CreateIconSets();
      config.MultiApps = Options.Applications.EnableMultiple;

      BackofficeUser user = await AuthenticationApi.GetUser();

      config.Translations = CreateTranslations(user?.LanguageId);

      if (user != null)
      {
        config.User = new UserEditModel()
        {
          Id = user.Id,
          AvatarId = user.AvatarId,
          Email = user.Email,
          IsSuper = user.IsSuper,
          Name = user.Name,
          Roles = user.RoleIds
        };

        config.Sections = CreateSections();
        config.Applications = await CreateApplications();
        config.SettingsAreas = CreateSettingsAreas();

        config.Plugins = Plugins.Select(x => new ZeroVuePlugin()
        {
          Name = x.Options.Name,
          Description = x.Options.Description,
          PluginPath = x.Options.PluginPath
        }).ToList();

        config.Services = new()
        {
          YouTubeApiKey = Options.Services.YouTubeApiKey
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


    /// <inheritdoc/>
    public string GetIconSvg()
    {
      return IconSymbolsSvg;
    }


    /// <summary>
    /// Creates the sections
    /// </summary>
    IList<ZeroVueSection> CreateSections()
    {
      bool isSuperUser = AuthenticationApi.IsSuper();
      IList<Permission> permissions = AuthenticationApi.GetPermissions(Permissions.Sections.PREFIX);

      List<ZeroVueSection> sections = new List<ZeroVueSection>();

      foreach (ISection section in Options.Sections.GetAllItems())
      {
        if (!isSuperUser && !Permission.CanReadKey(permissions, section.Alias, true))
        {
          continue;
        }

        bool isExternal = !(section is IInternalSection);
        string url = Safenames.Alias(section.Alias).EnsureStartsWith('/');

        if (section.Alias == Constants.Sections.Dashboard)
        {
          url = "/";
        }

        ZeroVueSection vueSection = new ZeroVueSection()
        {
          Alias = section.Alias,
          Name = section.Name,
          Icon = section.Icon,
          Color = section.Color,
          Url = url,
          Children = new List<ZeroVueSection>(),
          IsExternal = isExternal
        };

        foreach (ISection child in section.Children)
        {
          vueSection.Children.Add(new ZeroVueSection()
          {
            Alias = child.Alias,
            Name = child.Name,
            Url = vueSection.Url.EnsureEndsWith('/') + Safenames.Alias(child.Alias),
            IsExternal = isExternal
          });
        }

        sections.Add(vueSection);
      }

      return sections;
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
    /// Creates the areas in the settings section
    /// </summary>
    IList<ZeroVueSettingsGroup> CreateSettingsAreas()
    {
      bool isSuperUser = AuthenticationApi.IsSuper();
      IList<Permission> permissions = AuthenticationApi.GetPermissions(Permissions.Settings.PREFIX);

      List<ZeroVueSettingsGroup> groups = new List<ZeroVueSettingsGroup>();

      bool hasIntegrations = Options.Integrations.GetAllItems().Any();

      foreach (SettingsGroup group in Options.Settings.GetAllItems())
      {
        List<ZeroVueSettingsArea> areas = new List<ZeroVueSettingsArea>();

        foreach (SettingsArea area in group.Items)
        {
          if (!isSuperUser && !Permission.CanReadKey(permissions, area.Alias, true))
          {
            continue;
          }
          if (area.Alias == Constants.Settings.Integrations && !hasIntegrations)
          {
            continue;
          }

          bool isPlugin = !(area is InternalSettingsArea);

          ZeroVueSettingsArea vueArea = new ZeroVueSettingsArea()
          {
            Alias = area.Alias,
            Name = area.Name,
            Description = area.Description,
            Icon = area.Icon,
            Url = Constants.Sections.Settings.EnsureStartsWith('/') + Safenames.Alias(area.Alias).EnsureStartsWith('/'),
            IsPlugin = isPlugin
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

      return groups;
    }


    /// <summary>
    /// Get all visible applications
    /// </summary>
    async Task<List<ZeroVueApplication>> CreateApplications()
    {
      IList<Application> applications = await ApplicationsApi.GetAll();

      string[] mediaIds = applications.Select(x => x.IconId).Where(x => x != null).ToArray();
      Dictionary<string, Media> media = await Store.Session().LoadAsync<Media>(mediaIds);

      return applications.OrderBy(app => app.Sort).Select(app => new ZeroVueApplication()
      {
        Id = app.Id,
        Name = app.Name,
        Image = app.IconId.IsNullOrEmpty() ? null : media.GetValueOrDefault(app.IconId)?.ThumbnailSource
      }).ToList();
    }


    /// <summary>
    /// Creates all translations for the project
    /// </summary>
    Dictionary<string, string> CreateTranslations(string culture)
    {
      var zeroTranslations = CreateTranslationsForFile("O:/zero/zero.Web/Resources/Localization/zero.en-us.json", culture); // TODO

      foreach (IZeroPlugin plugin in Plugins)
      {
        if (plugin.Options.LocalizationPaths.Count > 0)
        {
          foreach (string path in plugin.Options.LocalizationPaths)
          {
            Dictionary<string, string> translations = CreateTranslationsForFile(path, culture);

            foreach (var translation in translations)
            {
              zeroTranslations.Add(translation.Key, translation.Value);
            }
          }
        }
      }

      return zeroTranslations;
    }


    Dictionary<string, string> CreateTranslationsForFile(string path, string culture)
    {
      Dictionary<string, string> items = new();
      culture = culture?.ToLower();

      if (!culture.IsNullOrEmpty() && culture != "en-us")
      {
        items = CreateTranslationsForFile(path, "en-us");
        path = path.Replace("en-us", culture);
      }

      if (!File.Exists(path))
      {
        return items;
      }

      string text = File.ReadAllText(path, Encoding.GetEncoding("ISO-8859-1"));

      JObject json = JObject.Parse(text);
      IEnumerable<JToken> tokens = json.Descendants().Where(p => p.Count() == 0);

      Dictionary<string, string> translationItems = tokens.Aggregate(new Dictionary<string, string>(), (properties, token) =>
      {
        properties.Add(token.Path.ToLowerInvariant(), token.ToString());
        return properties;
      });

      foreach (var translation in translationItems)
      {
        items[translation.Key] = translation.Value;
      }

      return items;
    }


    IList<ZeroVueIconSet> CreateIconSets()
    {
      List<ZeroVueIconSet> result = new();
      IReadOnlyCollection<IconSet> sets = Options.Icons.GetAllItems();

      StringBuilder svg = new();

      foreach (IconSet set in sets)
      {
        string path = Paths.Map(set.SpritePath.TrimStart('/'));

        if (!File.Exists(path))
        {
          Logger.LogWarning("Could not load icon set {alias} from path {path}", set.Alias, path);
          continue;
        }

        string svgContent = File.ReadAllText(path, Encoding.UTF8);
        XDocument xml = XDocument.Parse(svgContent);
        IEnumerable<XElement> symbols = xml.Descendants().Where(x => x.Name.LocalName == "symbol"); // ("symbol");

        if (!symbols.Any())
        {
          Logger.LogWarning("Icon set {alias} does not contain any <symbol>", set.Alias);
          continue;
        }

        ZeroVueIconSet iconSet = new()
        {
          Alias = set.Alias,
          Name = set.Name,
          Prefix = set.Prefix
        };

        foreach (XElement symbol in symbols)
        {
          string symbolAlias = set.Prefix + "-" + symbol.Attribute("id").Value.ToString();
          symbol.SetAttributeValue("id", symbolAlias);
          svg.Append(symbol.ToString().RemoveNewLines());
          iconSet.Icons.Add(symbolAlias);
        }

        result.Add(iconSet);
      }

      IconSymbolsSvg = svg.ToString();

      return result;
    }


    List<ZeroVueBlueprint> CreateBlueprints()
    {
      List<ZeroVueBlueprint> items = new();

      if (!Options.Blueprints.Enabled)
      {
        return items;
      }

      foreach (Blueprint blueprint in Options.Blueprints.GetAllItems())
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

    ///// <summary>
    ///// Creates the zero configuration for vue
    ///// </summary>
    //Task<Dictionary<string, string>> Translations();

    ///// <summary>
    ///// Creates the zero configuration for vue
    ///// </summary>
    //Task<ZeroVueConfig> IconSets();

    /// <summary>
    /// Creates the zero configuration for vue
    /// </summary>
    Task<string> ConfigAsJson();

    /// <summary>
    /// Get SVG for icon sets
    /// </summary>
    string GetIconSvg();
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

    public UserEditModel User { get; set; }

    public IList<ZeroVueSection> Sections { get; set; } = new List<ZeroVueSection>();

    public IList<ZeroVueApplication> Applications { get; set; } = new List<ZeroVueApplication>();

    public IList<ZeroVueSettingsGroup> SettingsAreas { get; set; } = new List<ZeroVueSettingsGroup>();

    public Dictionary<string, Dictionary<string, string>> Alias { get; set; } = new Dictionary<string, Dictionary<string, string>>();

    public Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();

    public Dictionary<string, object> Overrides { get; set; } = new Dictionary<string, object>();

    public IList<ZeroVueIconSet> Icons { get; set; } = new List<ZeroVueIconSet>();

    public ZeroVueServices Services { get; set; } = new();

    public IList<ZeroVueBlueprint> Blueprints { get; set; } = new List<ZeroVueBlueprint>();
  }


  public class ZeroVueSection
  {
    public string Alias { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public string Color { get; set; }

    public string Url { get; set; }

    public bool IsExternal { get; set; }

    public IList<ZeroVueSection> Children { get; set; }
  }


  public class ZeroVuePlugin
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public string PluginPath { get; set; }
  }


  public class ZeroVueSettingsGroup
  {
    public string Name { get; set; }

    public IList<ZeroVueSettingsArea> Items { get; set; }
  }


  public class ZeroVueSettingsArea
  {
    public string Alias { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Icon { get; set; }

    public string Url { get; set; }

    public bool IsPlugin { get; set; }
  }


  public class ZeroVueApplication
  {
    public string Id { get; set; }

    public string Name { get; set; }

    public string Image { get; set; }
  }

  public class ZeroVueIconSet
  {
    public string Alias { get; set; }

    public string Name { get; set; }

    public string Prefix { get; set; }

    public HashSet<string> Icons { get; set; } = new();
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
}
