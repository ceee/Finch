using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Messages;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Core.Routing;
using zero.Core.Validation;
using zero.Web.Routing;
using zero.Web.Sections;

namespace zero.Web.Defaults
{
  internal class ZeroBackofficePlugin : ZeroPlugin
  {
    public ZeroBackofficePlugin()
    {
      Options.Name = "zero.Defaults";
      Options.LocalizationPaths.Add("~/Resources/Localization/zero.{lang}.json");
    }


    public override void Configure(IZeroOptions zero)
    {
      zero.Sections.Add<DashboardSection>();
      zero.Sections.Add<PagesSection>();
      zero.Sections.Add<SpacesSection>();
      zero.Sections.Add<MediaSection>();
      zero.Sections.Add<SettingsSection>();

      zero.Settings.AddGroup<SystemSettings>();
      //zero.Settings.AddGroup<PluginSettings>();

      zero.Permissions.AddCollection<SectionPermissions>();
      zero.Permissions.AddCollection<ModulePermissions>();
      zero.Permissions.AddCollection<SettingsPermissions>();
      zero.Permissions.AddCollection<SpacePermissions>();
    }


    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) 
    {
      //services.AddAll(typeof(IValidator<>), ServiceLifetime.Scoped);
      //services.AddAll(typeof(IValidator), ServiceLifetime.Scoped);
      services.AddSingleton<IMessageAggregator, MessageAggregator>();

      services.AddTransient<IApplication, Application>();
      services.AddTransient<ICountry, Country>();
      services.AddTransient<ILanguage, Language>();
      services.AddTransient<ITranslation, Translation>();
      services.AddTransient<IPage, Page>();
      services.AddTransient<IRecycledEntity, RecycledEntity>();
      services.AddTransient<IMedia, Media>();
      services.AddTransient<IMediaFolder, MediaFolder>();
      services.AddTransient<IPreview, Preview>();
      services.AddTransient<IRoute, Route>();

      services.AddTransient<IValidator<IApplication>, ApplicationValidator>();
      services.AddTransient<IValidator<ICountry>, CountryValidator>();
      services.AddTransient<IValidator<ILanguage>, LanguageValidator>();
      services.AddTransient<IValidator<ITranslation>, TranslationValidator>();
      services.AddTransient<IValidator<IPage>, PageValidator>();
      services.AddTransient<IValidator<IMedia>, MediaValidator>();
      services.AddTransient<IValidator<IMediaFolder>, MediaFolderValidator>();
      services.AddTransient<IValidator<IUserRole>, UserRoleValidator>();
      services.AddTransient<IValidator<IUser>, BackofficeUserValidator>();

      services.AddTransient<IApplicationsApi, ApplicationsApi>();
      services.AddTransient<ICountriesApi, CountriesApi>();
      services.AddTransient<ILanguagesApi, LanguagesApi>();
      services.AddTransient<ITranslationsApi, TranslationsApi>();
      services.AddTransient<IUserApi, UserApi>();
      services.AddTransient<IPagesApi, PagesApi>();
      services.AddTransient<IPageTreeApi, PageTreeApi>();
      services.AddTransient<IPreviewApi, PreviewApi>();

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
      services.AddTransient<IModulesApi, ModulesApi>();
      services.AddTransient<IRecycleBinApi, RecycleBinApi>();

      services.AddScoped<IRoutes, Routes>();
      services.AddScoped<IPageUrlBuilder, PageUrlBuilder>();
      //services.AddTransient<IUrlProvider, PageUrlProvider>();
      services.AddScoped<IRouteProvider, PageRouteProvider>();
      services.AddScoped<PageRouteProvider>();

      services.AddScoped<ZeroRoutesTransformer>();
    }
  }
}