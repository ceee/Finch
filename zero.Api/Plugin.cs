using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using zero.Api.Endpoints.Countries;
using zero.Api.Endpoints.Languages;
using zero.Api.Endpoints.Search;

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
    services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ZeroApiMvcOptions>());

    services.AddScoped<IZeroMapper, ZeroMapper>();

    ZeroModuleCollection.AddModule<Endpoints.Applications.ApplicationModule>(services, configuration);
    ZeroModuleCollection.AddModule<CountryModule>(services, configuration);
    ZeroModuleCollection.AddModule<LanguageModule>(services, configuration);
    ZeroModuleCollection.AddModule<SearchModule>(services, configuration);

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