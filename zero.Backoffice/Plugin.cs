using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.IO;

namespace zero.Backoffice;

public class ZeroBackofficePlugin : ZeroPlugin
{
  internal Action<IServiceCollection, IConfiguration> PostConfigureServices = null;


  public ZeroBackofficePlugin()
  {
    Options.Name = "zero.Backoffice";
    Options.LocalizationPaths.Add("~/Resources/Localization/zero.{lang}.json");
  }


  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<BackofficeOptions>().Bind(configuration.GetSection("Zero:Backoffice")).Configure<IWebHostEnvironment>(ConfigureOptions);
    services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ZeroBackofficeMvcOptions>());
    services.AddHostedService<ZeroDevService>();
    services.AddTransient<IZeroVue, ZeroVue>();

    //Mvc.AddNewtonsoftJson(x =>
    //{
    //  // TODO this shall only be configurated for backoffice controllers
    //  BackofficeJsonSerlializerSettings.Setup(x.SerializerSettings);
    //});

    services.AddZeroBackofficeUIComposition();

    //services.AddTransient<ISectionsApi, SectionsApi>();
    //services.AddTransient<ISettingsApi, SettingsApi>();

    //services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();

    PostConfigureServices?.Invoke(services, configuration);
  }


  protected void ConfigureOptions(BackofficeOptions options, IWebHostEnvironment env)
  {
    options.Search.Enabled = true;
    options.DevServer.WorkingDirectory = Path.Combine(env.ContentRootPath, "..", "Zero.Web.UI", "App");

    options.IconSets.Add(new BackofficeIconSet()
    {
      Alias = "feather",
      Name = "Feather",
      SpritePath = "/assets/icons/feather.svg",
      Prefix = "fth"
    });

    options.SupportedLanguages = new string[2] { "en-US", "de-DE" };
    options.DefaultLanguage = options.SupportedLanguages[0];

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