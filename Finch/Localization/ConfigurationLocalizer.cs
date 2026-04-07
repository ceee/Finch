using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Finch.Localization;

public class ConfigurationLocalizer : Localizer
{
  protected ConcurrentDictionary<string, Translation> FileCache { get; set; } = [];


  public ConfigurationLocalizer(IConfiguration configuration, ICultureResolver cultureResolver, IOptionsMonitor<LocalizationOptions> options) : base(cultureResolver)
  {
    IConfigurationSection section = configuration.GetSection($"Finch:Localization:{LanguageCode}");

    if (section != null)
    {
      foreach (IConfigurationSection child in section.GetChildren())
      {
        FileCache.TryAdd(child.Key, new Translation() { Value = child.Value });
      }
    }
  }


  protected override Translation LoadTranslation(string key)
  {
    if (!FileCache.TryGetValue(key, out Translation translation))
    {
      return null;
    }

    return translation;
  }
}