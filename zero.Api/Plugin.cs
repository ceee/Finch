using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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
    services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ZeroApiMvcOptions>());
    services.AddTransient<IBackofficeApplicationResolverHandler, ApiApplicationResolverHandler>();

    ZeroModuleCollection modules = new();

    modules.Add<Endpoints.Applications.ApplicationModule>();
    modules.Add<Endpoints.Countries.CountryModule>();
    modules.Add<Endpoints.Languages.LanguageModule>();
    modules.Add<Endpoints.Search.SearchModule>();
    modules.Add<Endpoints.Translations.TranslationModule>();
    modules.Add<Endpoints.Mails.MailModule>();
    modules.Add<Endpoints.Pages.PageModule>();
    modules.Add<Endpoints.Media.MediaModule>();
    modules.Add<Endpoints.Spaces.SpaceModule>();

    modules.ConfigureServices(services, configuration);
      
    PostConfigureServices?.Invoke(services, configuration);
  }


  public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
  {
    string path = "/zero/api";

    app.UseWhen(ctx => ctx.Request.Path.StartsWithSegments(path), appScoped =>
    {
      appScoped.UseEndpoints(endpoints =>
      {
        //IZeroOptions options = app.ApplicationServices.GetService<IZeroOptions>(); // TODO oO
        // see https://our.umbraco.com/documentation/reference/routing/custom-routes#where-to-put-your-routing-logic
        //string path = options.BackofficePath.EnsureStartsWith('/').TrimEnd('/');
        //endpoints.MapFallbackToController(path + "/{**path}", "Index", "ZeroIndex");

        //endpoints.MapControllers();
      });
    });
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