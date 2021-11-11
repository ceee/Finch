using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using zero.Core;
using zero.Core.Api;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Integrations;
using zero.Core.Mails;
using zero.Core.Messages;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Core.Renderer;
using zero.Core.Routing;
using zero.Core.Services;
using zero.Core.Tokens;
using zero.Core.Validation;
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
      zero.Settings.AddGroup<PluginSettings>();

      zero.Permissions.AddCollection<SectionPermissions>();
      zero.Permissions.AddCollection<ModulePermissions>();
      zero.Permissions.AddCollection<SettingsPermissions>();
      zero.Permissions.AddCollection<SpacePermissions>();

      zero.Icons.AddSet("feather", "Feather", "/assets/icons/feather.svg", "fth");
      zero.Pages.Add<PageFolder>(Constants.Pages.FolderAlias, "@page.folder.name", "@page.folder.description", "fth-folder");

      zero.Interceptors.Add<ZeroEntityRouteInterceptor>(gravity: 1000);
    }


    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) 
    {
      //services.AddAll(typeof(IValidator<>), ServiceLifetime.Scoped);
      //services.AddAll(typeof(IValidator), ServiceLifetime.Scoped);
      services.AddSingleton<IMessageAggregator, MessageAggregator>();

      services.AddScoped<IRazorRenderer, RazorRenderer>();

      services.AddTransient<Application, Application>();
      services.AddTransient<Country, Country>();
      services.AddTransient<Language, Language>();
      services.AddTransient<Translation, Translation>();
      services.AddTransient<Page, Page>();
      services.AddTransient<RecycledEntity, RecycledEntity>();
      services.AddTransient<Media, Media>();
      services.AddTransient<MediaFolder, MediaFolder>();
      services.AddTransient<Preview, Preview>();
      services.AddTransient<Core.Routing.Route, Core.Routing.Route>();

      services.AddTransient<IValidator<Application>, ApplicationValidator>();
      services.AddTransient<IValidator<Country>, CountryValidator>();
      services.AddTransient<IValidator<MailTemplate>, MailTemplateValidator>();
      services.AddTransient<IValidator<Language>, LanguageValidator>();
      services.AddTransient<IValidator<Translation>, TranslationValidator>();
      services.AddTransient<IValidator<Page>, PageValidator>();
      services.AddTransient<IValidator<Media>, MediaValidator>();
      services.AddTransient<IValidator<MediaFolder>, MediaFolderValidator>();
      services.AddTransient<IValidator<BackofficeUserRole>, UserRoleValidator>();
      services.AddTransient<IValidator<BackofficeUser>, BackofficeUserValidator>();

      services.AddTransient<IApplicationsApi, ApplicationsApi>();
      services.AddTransient<ICountriesCollection, CountriesCollection>();
      services.AddTransient<ILanguagesCollection, LanguagesCollection>();
      services.AddTransient<ITranslationsCollection, TranslationsCollection>();
      services.AddTransient<IMediaCollection, MediaCollection>();
      services.AddTransient<IPagesCollection, PagesCollection>();
      services.AddTransient<IUserApi, UserApi>();
      services.AddTransient<IPreviewApi, PreviewApi>();
      services.AddTransient<IMailTemplatesCollection, MailTemplatesCollection>();

      services.AddTransient<ISetupApi, SetupApi>();
      services.AddTransient<ISectionsApi, SectionsApi>();
      services.AddTransient<ISettingsApi, SettingsApi>();
      services.AddTransient<IAuthenticationApi, AuthenticationApi>();
      services.AddTransient<IUserRolesApi, UserRolesApi>();
      services.AddTransient<ISpacesApi, SpacesApi>();
      services.AddTransient<IPermissionsApi, PermissionsApi>();
      services.AddTransient<IMediaApi, MediaApi>();
      services.AddTransient<IMediaFolderApi, MediaFolderApi>();
      services.AddTransient<IModulesApi, ModulesApi>();
      services.AddTransient<IRecycleBinApi, RecycleBinApi>();

      services.AddScoped<IRequestUrlResolver, RequestUrlResolver>();
      services.AddScoped<IRoutes, Routes>();
      services.AddScoped<IRouteResolver, RouteResolver>();
      services.AddScoped<IRedirectAutomation, RedirectAutomation>();
      services.AddScoped<IRouteRedirectCollection, RouteRedirectCollection>();
      services.AddScoped<IPageUrlBuilder, PageUrlBuilder>();
      services.AddScoped<IRouteProvider, PageRouteProvider>();
      services.AddScoped<ILinks, Links>();
      services.AddScoped<ILinkProvider, PageLinkProvider>();
      services.AddScoped<ILinkProvider, RawLinkProvider>();
      services.AddScoped<ZeroRoutesTransformer>();
      services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, NotFoundSelectorPolicy>());

      services.AddScoped<IMailProvider, MailProvider>();
      services.AddScoped<IMailDispatcher, FileMailDispatcher>();

      services.AddScoped<IZeroMediaHelper, ZeroMediaHelper>();

      services.AddTransient<IIntegrationService, IntegrationService>();
      services.AddTransient<IIntegrationsCollection, IntegrationsCollection>();

      services.AddScoped<ICollectionContext, CollectionContext>();

      services.AddScoped<ILocalizer, Localizer>();
      services.AddScoped<IZeroTokenProvider, ZeroTokenProvider>();

      services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();

      services.AddScoped<ZeroEntityRouteInterceptor>();
    }
  }
}