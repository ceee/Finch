using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
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
using zero.Web.Sections;

namespace zero.Web
{
  public class ZeroVue : IZeroVue
  {
    protected IZeroConfiguration Config { get; private set; }

    protected IWebHostEnvironment Environment { get; private set; }

    protected IApplicationsApi ApplicationsApi { get; private set; }

    protected IUserApi UserApi { get; private set; }

    protected ZeroOptions Options { get; private set; }


    public ZeroVue(IZeroConfiguration config, IWebHostEnvironment env, IOptionsMonitor<ZeroOptions> options, IApplicationsApi applicationsApi, IUserApi userApi)
    {
      config = Config;
      Environment = env;
      Options = options.CurrentValue;
      ApplicationsApi = applicationsApi;
      UserApi = userApi;
      //zero.path = "@Model.BackofficePath.EnsureEndsWith('/')";
      //zero.translations = @Html.Raw(text);
    }


    /// <inheritdoc/>
    public async Task<string> ConfigAsJson()
    {
      ZeroVueConfig config = new ZeroVueConfig();

      config.Path = Options.BackofficePath.EnsureEndsWith('/');
      config.ApiPath = config.Path + "api/";
      config.PluginPath = "@/Plugins";
      config.ErrorFieldNone = Constants.ErrorFieldNone;
      config.Sections = CreateSections();
      config.Translations = CreateTranslations();
      config.Applications = await CreateApplications();
      config.Alias = CreateAliases();
      config.SettingsAreas = CreateSettingsAreas();

      config.User = await UserApi.GetUser();

      return JsonSerializer.Serialize(config, new JsonSerializerOptions()
      {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });
    }


    /// <summary>
    /// Creates the sections
    /// </summary>
    IList<ZeroVueSection> CreateSections()
    {
      bool isSuperUser = UserApi.IsSuper();
      IList<Permission> permissions = UserApi.GetPermissions(Permissions.Sections.PREFIX);

      List<ZeroVueSection> sections = new List<ZeroVueSection>();

      foreach (ISection section in Options.Sections)
      {
        if (!isSuperUser && !Permission.CanReadKey(permissions, section.Alias, true))
        {
          continue;
        }

        bool isExternal = !(section is IBuiltInSection);
        string url = Alias.Generate(section.Alias).EnsureStartsWith('/');

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
            Url = vueSection.Url.EnsureEndsWith('/') + Alias.Generate(child.Alias),
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
      sections.Add("lists", Constants.Sections.Lists);
      sections.Add("media", Constants.Sections.Media);
      sections.Add("settings", Constants.Sections.Settings);

      Dictionary<string, string> settings = new Dictionary<string, string>();
      settings.Add("applications", Constants.Settings.Applications);
      settings.Add("countries", Constants.Settings.Countries);
      settings.Add("logging", Constants.Settings.Logging);
      settings.Add("translations", Constants.Settings.Translations);
      settings.Add("updates", Constants.Settings.Updates);
      settings.Add("users", Constants.Settings.Users);

      aliases.Add("sections", sections);
      aliases.Add("settings", settings);

      return aliases;
    }


    /// <summary>
    /// Creates the areas in the settings section
    /// </summary>
    IList<ZeroVueSettingsGroup> CreateSettingsAreas()
    {
      bool isSuperUser = UserApi.IsSuper();
      IList<Permission> permissions = UserApi.GetPermissions(Permissions.Settings.PREFIX);

      List<ZeroVueSettingsGroup> groups = new List<ZeroVueSettingsGroup>();

      foreach (SettingsGroup group in Options.SettingsAreas)
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
            Url = Constants.Sections.Settings.EnsureStartsWith('/') + Alias.Generate(area.Alias).EnsureStartsWith('/')
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
      string path = Path.Combine(Environment.ContentRootPath, "Resources/Localization/zero.en-us.json");
      string text = File.ReadAllText(path, Encoding.UTF8);

      JObject json = JObject.Parse(text);
      IEnumerable<JToken> tokens = json.Descendants().Where(p => p.Count() == 0);

      return tokens.Aggregate(new Dictionary<string, string>(), (properties, token) =>
      {
        properties.Add(token.Path, token.ToString());
        return properties;
      });
    }
  }


  public interface IZeroVue
  {
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

    public string ErrorFieldNone { get; set; }

    public User User { get; set; }

    public IList<ZeroVueSection> Sections { get; set; } = new List<ZeroVueSection>();

    public IList<ZeroVueApplication> Applications { get; set; } = new List<ZeroVueApplication>();

    public IList<ZeroVueSettingsGroup> SettingsAreas { get; set; } = new List<ZeroVueSettingsGroup>();

    public Dictionary<string, Dictionary<string, string>> Alias { get; set; } = new Dictionary<string, Dictionary<string, string>>();

    public Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();
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
  }


  public class ZeroVueApplication
  {
    public string Id { get; set; }

    public string Name { get; set; }
  }
}
