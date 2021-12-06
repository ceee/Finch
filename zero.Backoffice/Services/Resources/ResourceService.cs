using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace zero.Backoffice.Services;

public class ResourceService : IResourceService
{
  protected IOptions<BackofficeOptions> Options { get; set; }

  protected IBackofficeAssetFileSystem FileSystem { get; set; }

  protected ILogger<IconService> Logger { get; set; }

  protected IEnumerable<IZeroPlugin> Plugins { get; set; }


  public ResourceService(IOptions<BackofficeOptions> options, IBackofficeAssetFileSystem fileSystem, ILogger<IconService> logger, IEnumerable<IZeroPlugin> plugins)
  {
    Options = options;
    FileSystem = fileSystem;
    Logger = logger;
    Plugins = plugins;
  }


  /// <inheritdoc />
  public Task<Dictionary<string, string>> GetTranslations(string cultureIsoCode)
  {
    var zeroTranslations = CreateTranslationsForFile("O:/zero/zero.Backoffice/Resources/Localization/zero.en-us.json", cultureIsoCode); // TODO

    foreach (IZeroPlugin plugin in Plugins)
    {
      if (plugin.Options.LocalizationPaths.Count > 0)
      {
        foreach (string path in plugin.Options.LocalizationPaths)
        {
          Dictionary<string, string> translations = CreateTranslationsForFile(path, cultureIsoCode);

          foreach (var translation in translations)
          {
            zeroTranslations.Add(translation.Key, translation.Value);
          }
        }
      }
    }

    return Task.FromResult(zeroTranslations);
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
    IEnumerable<JToken> tokens = json.Descendants().Where(p => !p.Any());

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
}


public interface IResourceService
{
  /// <summary>
  /// Get all backoffice translations from all registered plugins
  /// </summary>
  Task<Dictionary<string, string>> GetTranslations(string cultureIsoCode);
}