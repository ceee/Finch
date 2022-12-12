using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace zero.Localization;

public class ConfigurationLocalizer : Localizer
{
  private IConfiguration _configuration;


  public ConfigurationLocalizer(IConfiguration configuration, IOptionsMonitor<LocalizationOptions> options) : base()
  {
    _configuration = configuration;
  }


  protected override Translation LoadTranslation(string key)
  {
    IConfigurationSection section = _configuration.GetSection($"Zero:Localization:{key}");

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