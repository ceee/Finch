using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
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

    protected IWebHostEnvironment Environment { get; private set; }

    protected IApplicationsApi ApplicationsApi { get; private set; }

    protected IAuthenticationApi AuthenticationApi { get; private set; }

    protected IEnumerable<IZeroPlugin> Plugins { get; private set; }

    protected IZeroContext Context { get; private set; }


    public ZeroVue(IZeroOptions options, IWebHostEnvironment env, IApplicationsApi applicationsApi, IAuthenticationApi authenticationApi, IEnumerable<IZeroPlugin> plugins, IZeroContext context)
    {
      Environment = env;
      Options = options;
      ApplicationsApi = applicationsApi;
      AuthenticationApi = authenticationApi;
      Plugins = plugins;
      Context = context;
      //zero.path = "@Model.BackofficePath.EnsureEndsWith('/')";
      //zero.translations = @Html.Raw(text);
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
      config.Sections = CreateSections();
      config.Translations = CreateTranslations();
      config.Applications = await CreateApplications();
      config.Alias = CreateAliases();
      config.SettingsAreas = CreateSettingsAreas();
      config.AppId = Context.AppId;
      //config.SharedAppId = Constants.Database.SharedAppId; // TODO appx

      BackofficeUser user = await AuthenticationApi.GetUser();

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
      }

      config.Plugins = Plugins.Select(x => new ZeroVuePlugin()
      {
        Name = x.Options.Name,
        Description = x.Options.Description,
        PluginPath = x.Options.PluginPath
      }).ToList();

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

        bool isExternal = !(section is IZeroInternal);
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

          bool isPlugin = !(area is IZeroInternal);

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
      IList<IApplication> applications = await ApplicationsApi.GetAll();

      return applications.OrderBy(app => app.Sort).Select(app => new ZeroVueApplication()
      {
        Id = app.Id,
        Name = app.Name
      }).ToList();
    }


    /// <summary>
    /// Creates all translations for the project
    /// </summary>
    Dictionary<string, string> CreateTranslations()
    {
      var zeroTranslations = CreateTranslationsForFile("O:/zero/zero.Web/Resources/Localization/zero.en-us.json"); // TODO

      foreach (IZeroPlugin plugin in Plugins)
      {
        if (plugin.Options.LocalizationPaths.Count > 0)
        {
          foreach (string path in plugin.Options.LocalizationPaths)
          {
            Dictionary<string, string> translations = CreateTranslationsForFile(path);

            foreach (var translation in translations)
            {
              zeroTranslations.Add(translation.Key, translation.Value);
            }
          }
        }
      }

      return zeroTranslations;
    }


    Dictionary<string, string> CreateTranslationsForFile(string path)
    {
      if (!File.Exists(path))
      {
        return new Dictionary<string, string>();
      }
      string text = File.ReadAllText(path, Encoding.UTF8);

      JObject json = JObject.Parse(text);
      IEnumerable<JToken> tokens = json.Descendants().Where(p => p.Count() == 0);

      return tokens.Aggregate(new Dictionary<string, string>(), (properties, token) =>
      {
        properties.Add(token.Path.ToLowerInvariant(), token.ToString());
        return properties;
      });
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

    public string SharedAppId { get; set; }

    public UserEditModel User { get; set; }

    public IList<ZeroVueSection> Sections { get; set; } = new List<ZeroVueSection>();

    public IList<ZeroVueApplication> Applications { get; set; } = new List<ZeroVueApplication>();

    public IList<ZeroVueSettingsGroup> SettingsAreas { get; set; } = new List<ZeroVueSettingsGroup>();

    public Dictionary<string, Dictionary<string, string>> Alias { get; set; } = new Dictionary<string, Dictionary<string, string>>();

    public Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();

    public Dictionary<string, object> Overrides { get; set; } = new Dictionary<string, object>();
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
  }
}
