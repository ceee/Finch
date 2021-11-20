using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using zero.Web.Sections;
using zero.Web.ViewHelpers;

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
      zero.Settings.AddGroup<ApplicationSettings>();

      zero.Permissions.AddCollection<SectionPermissions>();
      zero.Permissions.AddCollection<ModulePermissions>();
      zero.Permissions.AddCollection<SettingsPermissions>();
      zero.Permissions.AddCollection<SpacePermissions>();

      zero.Icons.AddSet("feather", "Feather", "/assets/icons/feather.svg", "fth");
      zero.Pages.Add<PageFolder>(Constants.Pages.FolderAlias, "@page.folder.name", "@page.folder.description", "fth-folder");

      zero.Interceptors.Add<ZeroEntityRouteInterceptor, ZeroEntity>(gravity: 100);
      zero.Interceptors.Add<BlueprintInterceptor>(gravity: -1);
      zero.Interceptors.Add<BlueprintChildInterceptor>(gravity: -1);
    }


    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) 
    {
      //services.AddAll(typeof(IValidator<>), ServiceLifetime.Scoped);
      //services.AddAll(typeof(IValidator), ServiceLifetime.Scoped);

      services.AddTransient<ICountriesCollection, CountriesCollection>();
      services.AddTransient<ILanguagesCollection, LanguagesCollection>();
      services.AddTransient<ITranslationsCollection, TranslationsCollection>();
      services.AddTransient<IMediaCollection, MediaCollection>();
      services.AddTransient<IMediaFolderCollection, MediaFolderCollection>();
      services.AddTransient<IPagesCollection, PagesCollection>();
      services.AddTransient<IMailTemplatesCollection, MailTemplatesCollection>();

      services.AddTransient<IApplicationsApi, ApplicationsApi>();
      services.AddTransient<IUserApi, UserApi>();
      services.AddTransient<IPreviewApi, PreviewApi>();

      services.AddTransient<ISetupApi, SetupApi>();
      services.AddTransient<ISectionsApi, SectionsApi>();
      services.AddTransient<ISettingsApi, SettingsApi>();
      services.AddTransient<IAuthenticationApi, AuthenticationApi>();
      services.AddTransient<IUserRolesApi, UserRolesApi>();
      services.AddTransient<ISpacesApi, SpacesApi>();
      services.AddTransient<IPermissionsApi, PermissionsApi>();
      services.AddTransient<IModulesApi, ModulesApi>();
      services.AddTransient<IRecycleBinApi, RecycleBinApi>();  

      services.AddScoped<IZeroMediaHelper, ZeroMediaHelper>();

      services.AddTransient<IIntegrationService, IntegrationService>();
      services.AddTransient<IIntegrationsCollection, IntegrationsCollection>();

      services.AddScoped<ICollectionContext, CollectionContext>();
      services.AddScoped(typeof(ICollectionContext<>), typeof(CollectionContext<>));
      services.AddScoped(typeof(IInterceptorRunner<>), typeof(InterceptorRunner<>));

      services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();
    }
  }
}