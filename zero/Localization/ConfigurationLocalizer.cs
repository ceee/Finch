using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace zero.Localization;

public class ConfigurationLocalizer : Localizer
{
  private IConfiguration _configuration;


  public ConfigurationLocalizer(IConfiguration configuration, IOptionsMonitor<LocalizationOptions> options) : base()
  {
    _configuration = configuration;

    // set default language
    Language(_configuration.GetValue<string>("Zero:Localization:Default", "de"));
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