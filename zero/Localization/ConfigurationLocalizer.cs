using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace zero.Localization;

public class ConfigurationLocalizer : Localizer
{
  private IConfiguration _configuration;


  public ConfigurationLocalizer(IConfiguration configuration, ICultureResolver cultureResolver, IOptionsMonitor<LocalizationOptions> options) : base(cultureResolver)
  {
    _configuration = configuration;
  }


  protected override Translation LoadTranslation(string key)
  {
    IConfigurationSection section = _configuration.GetSection($"Zero:Localization:{LanguageCode}:{key}");

    if (section == null)
    {
      return null;
    }

    return new()
    {
      Key = key,
      Value = section.Value
    };
  }
}