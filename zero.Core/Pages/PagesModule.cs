using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Pages;

public class PagesModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IPagesStore, PagesStore>();
    services.AddScoped<IPageTypeService, PageTypeService>();
    services.AddScoped<IPageModuleTypeService, PageModuleTypeService>();

    services.AddOptions<PageOptions>().Bind(configuration.GetSection("Zero:Pages"));
    services.AddOptions<PageModuleOptions>().Bind(configuration.GetSection("Zero:PageModules"));

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<Pages_AsHistory>();
      opts.Indexes.Add<Pages_ByHierarchy>();
      opts.Indexes.Add<Pages_ByType>();
      opts.Indexes.Add<Pages_WithChildren>();
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