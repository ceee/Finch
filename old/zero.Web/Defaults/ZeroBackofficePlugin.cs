using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using zero.Web.ViewHelpers;

namespace zero.Web.Defaults
{
  internal class ZeroBackofficePlugin : ZeroPlugin
  {
    public override void Configure(IZeroOptions zero)
    {
      zero.Permissions.AddCollection<ModulePermissions>();

      zero.Pages.Add<PageFolder>(Constants.Pages.FolderAlias, "@page.folder.name", "@page.folder.description", "fth-folder");
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

      services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();
    }
  }
}