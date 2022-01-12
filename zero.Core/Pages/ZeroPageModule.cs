using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Pages;

internal class ZeroPageModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IPagesStore, PagesStore>();
    services.AddScoped<IPageTypeService, PageTypeService>();
    services.AddScoped<IPageModuleTypeService, PageModuleTypeService>();

    services.AddOptions<PageOptions>().Bind(configuration.GetSection("Zero:Pages"));
    services.AddOptions<PageModuleOptions>().Bind(configuration.GetSection("Zero:PageModules"));

    services.ConfigureOptions<ConfigureModuleTypeJsonOptions>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Pages_AsHistory>();
      opts.Indexes.Add<zero_Pages_ByHierarchy>();
      opts.Indexes.Add<zero_Pages_ByType>();
      opts.Indexes.Add<zero_Pages_WithChildren>();
    });

    services.Configure<FlavorOptions>(opts =>
    {
      opts.Configure<Page>(cfg =>
      {
        cfg.CanUseWithoutFlavors = false;
        cfg.Add<PageFolder>(Constants.Pages.FolderAlias, "@page.folder.name", "@page.folder.description", "fth-folder");
      });
    });
  }
}