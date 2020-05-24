using Microsoft.Extensions.DependencyInjection;
using zero.Core;
using zero.Core.Entities;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Web.Mapper;
using zero.Web.Sections;

namespace zero.Web.Defaults
{
  internal class DefaultBackofficePlugin : IZeroPlugin
  {
    public void ConfigureServices(IServiceCollection services) 
    {
      EntityMap.Use<IApplication, Application>();
      EntityMap.Use<ICountry, Country>();
    }
    
    public void Configure(IZeroPluginOptions plugin, IZeroOptions zero)
    {
      plugin.Name = "zero.Defaults";
      plugin.LocalizationPaths.Add("~/Resources/Localization/zero.{lang}.json");

      zero.Sections.Add<DashboardSection>();
      zero.Sections.Add<PagesSection>();
      zero.Sections.Add<SpacesSection>();
      zero.Sections.Add<MediaSection>();
      zero.Sections.Add<SettingsSection>();

      zero.Settings.AddGroup<SystemSettings>();
      zero.Settings.AddGroup<PluginSettings>();

      zero.Mapper.Add<UserMapperConfig>();
      zero.Mapper.Add<CountryMapperConfig>();
      zero.Mapper.Add<TranslationMapperConfig>();
      zero.Mapper.Add<LanguageMapperConfig>();
      zero.Mapper.Add<ApplicationMapperConfig>();
      zero.Mapper.Add<MediaMapperConfig>();
      zero.Mapper.Add<SpaceMapperConfig>();
    }
  }
}