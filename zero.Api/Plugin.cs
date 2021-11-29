using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace zero.Api;

public class ZeroApiPlugin : ZeroPlugin
{
  internal Action<IServiceCollection, IConfiguration> PostConfigureServices = null;


  public ZeroApiPlugin()
  {
    Options.Name = "zero.Api";
  }


  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<ApiOptions>().Bind(configuration.GetSection("Zero:Api")).Configure<IWebHostEnvironment>(ConfigureOptions);
    services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ZeroBackofficeMvcOptions>());

    //Mvc.AddNewtonsoftJson(x =>
    //{
    //  // TODO this shall only be configurated for backoffice controllers
    //  BackofficeJsonSerlializerSettings.Setup(x.SerializerSettings);
    //});

    services.AddScoped<IZeroMapper, ZeroMapper>();

    services.AddZeroBackofficeModules(configuration);

    //services.AddTransient<ISectionsApi, SectionsApi>();
    //services.AddTransient<ISettingsApi, SettingsApi>();

    //services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();


    PostConfigureServices?.Invoke(services, configuration);
  }


  protected void ConfigureOptions(ApiOptions options, IWebHostEnvironment env)
  {
    options.Search.Enabled = true;
    //Map<Page>().Display((x, res, opts) =>
    //{
    //  PageType pageType = opts.Pages.GetByAlias(x.PageTypeAlias);
    //  if (pageType != null)
    //  {
    //    res.Icon = pageType.Icon;
    //  }
    //  res.Url = "/pages/edit/" + x.Id;
    //});
    //Map<MediaFolder>("fth-image");
  }
}