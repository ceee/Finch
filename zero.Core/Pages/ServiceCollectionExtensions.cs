using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Pages;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroPages(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IPagesStore, PagesStore>();
    services.AddScoped<IPageTypeService, PageTypeService>();
    services.AddScoped<IPageModuleTypeService, PageModuleTypeService>();

    services.AddOptions<PageOptions>().Bind(config.GetSection("Zero:Pages"));
    services.AddOptions<PageModuleOptions>().Bind(config.GetSection("Zero:PageModules"));

    services.Configure<ZeroOptions>(opts =>
    {
      RavenOptions raven = opts.For<RavenOptions>();
      raven.Indexes.Add<Pages_AsHistory>();
      raven.Indexes.Add<Pages_ByHierarchy>();
      raven.Indexes.Add<Pages_ByType>();
      raven.Indexes.Add<Pages_WithChildren>();

      opts.For<PageOptions>().Add<PageFolder>(Constants.Pages.FolderAlias, "@page.folder.name", "@page.folder.description", "fth-folder");
    });
    return services;
  }
}