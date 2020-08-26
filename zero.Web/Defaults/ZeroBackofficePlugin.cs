using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using zero.Core.Api;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Web.Mapper;
using zero.Web.Sections;

namespace zero.Web.Defaults
{
  internal class ZeroBackofficePlugin : IZeroPlugin
  {
    public void ConfigureServices(IServiceCollection services) 
    {
      services.AddAll(typeof(IValidator<>), ServiceLifetime.Scoped);
      services.AddAll(typeof(IValidator), ServiceLifetime.Scoped);

      services.AddTransient<IApplication, Application>();
      services.AddTransient<ICountry, Country>();
      services.AddTransient<ILanguage, Language>();
      services.AddTransient<ITranslation, Translation>();
      services.AddTransient<IPage, Page>();
      services.AddTransient<IRecycledEntity, RecycledEntity>();

      services.AddTransient(typeof(IApplicationsApi<>), typeof(ApplicationsApi<>));
      services.AddTransient(typeof(ICountriesApi<>), typeof(CountriesApi<>));
      services.AddTransient(typeof(ILanguagesApi<>), typeof(LanguagesApi<>));
      services.AddTransient(typeof(ITranslationsApi), typeof(TranslationsApi));
      services.AddTransient(typeof(ITranslationsApi<>), typeof(TranslationsApi<>));
      services.AddTransient(typeof(ITranslationsApiFacade), typeof(TranslationsApiFacade));
      services.AddTransient(typeof(IPagesApi<>), typeof(PagesApi<>));
      services.AddTransient(typeof(IPageTreeApi<>), typeof(PageTreeApi<>));
      services.AddTransient(typeof(IUserApi<>), typeof(UserApi<>));
      services.AddTransient(typeof(IRecycleBinApi), typeof(RecycleBinApi));
     // services.AddTransient(typeof(IRecycleBinApi), typeof(RecycleBinApi<RecycledEntity>));

      services.AddTransient<ISetupApi, SetupApi>();
      services.AddTransient<ISectionsApi, SectionsApi>();
      services.AddTransient<ISettingsApi, SettingsApi>();
      services.AddTransient<IAuthenticationApi, AuthenticationApi>();
      services.AddTransient<IUserRolesApi, UserRolesApi>();
      services.AddTransient<IToken, Token>();
      services.AddTransient<ISpacesApi, SpacesApi>();
      services.AddTransient<IPermissionsApi, PermissionsApi>();
      services.AddTransient<IRevisionsApi, RevisionsApi>();
      services.AddTransient<IMediaApi, MediaApi>();
      services.AddTransient<IMediaFolderApi, MediaFolderApi>();
      services.AddTransient<IMediaUploadApi, MediaUploadApi>();
      services.AddTransient<IModulesApi, ModulesApi>();
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

      zero.Permissions.AddCollection<SectionPermissions>();
      zero.Permissions.AddCollection<SettingsPermissions>();
      zero.Permissions.AddCollection<SpacePermissions>();
    }
  }
}