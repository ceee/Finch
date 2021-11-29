using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
  }
}