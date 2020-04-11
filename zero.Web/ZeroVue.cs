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

namespace zero.Web
{
  public class ZeroVue : IZeroVue
  {
    protected IZeroConfiguration Config { get; private set; }

    protected IWebHostEnvironment Environment { get; private set; }

    protected IApplicationsApi ApplicationsApi { get; private set; }

    protected ZeroOptions Options { get; private set; }


    public ZeroVue(IZeroConfiguration config, IWebHostEnvironment env, IOptionsMonitor<ZeroOptions> options, IApplicationsApi applicationsApi)
    {
      config = Config;
      Environment = env;
      Options = options.CurrentValue;
      ApplicationsApi = applicationsApi;
      //zero.path = "@Model.BackofficePath.EnsureEndsWith('/')";
      //zero.translations = @Html.Raw(text);
    }


    /// <inheritdoc/>
    public async Task<string> ConfigAsJson()
    {
      ZeroVueConfig config = new ZeroVueConfig();

      config.Path = Options.BackofficePath.EnsureEndsWith('/');
      config.ApiPath = config.Path + "api/";
      config.Sections = CreateSections();
      config.Translations = CreateTranslations();
      config.Applications = await CreateApplications();

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
      List<ZeroVueSection> sections = new List<ZeroVueSection>();

      foreach (ISection section in Options.Sections)
      {
        ZeroVueSection vueSection = new ZeroVueSection()
        {
          Alias = section.Alias,
          Name = section.Name,
          Icon = section.Icon,
          Color = section.Color,
          Url = Alias.Generate(section.Alias).EnsureStartsWith('/'),
          Children = new List<ZeroVueSection>()
        };

        foreach (ISection child in section.Children)
        {
          vueSection.Children.Add(new ZeroVueSection()
          {
            Alias = child.Alias,
            Name = child.Name,
            Url = vueSection.Url.EnsureEndsWith('/') + Alias.Generate(child.Alias)
          });
        }

        sections.Add(vueSection);
      }

      return sections;
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

    public IList<ZeroVueSection> Sections { get; set; } = new List<ZeroVueSection>();

    public IList<ZeroVueApplication> Applications { get; set; } = new List<ZeroVueApplication>();

    public Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();
  }


  public class ZeroVueSection
  {
    public string Alias { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public string Color { get; set; }

    public string Url { get; set; }

    public IList<ZeroVueSection> Children { get; set; }
  }


  public class ZeroVueApplication
  {
    public string Id { get; set; }

    public string Name { get; set; }
  }
}
