using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.IO;
using zero.Backoffice.Endpoints.Account;
using zero.Backoffice.Endpoints.Applications;
using zero.Backoffice.Services;

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
    services.AddSingleton<IMapperProfile, AccountMapperProfile>();
    services.AddSingleton<IMapperProfile, ApplicationMapperProfile>();

    services.AddSingleton<IIconService, IconService>();
    services.AddSingleton<IResourceService, ResourceService>();
    services.AddSingleton<ISectionService, SectionService>();

    services.AddSingleton<IBackofficeAssetFileSystem, BackofficeAssetFileSystem>(svc =>
    {
      IOptions<BackofficeOptions> options = svc.GetRequiredService<IOptions<BackofficeOptions>>();
      IWebHostEnvironment env = svc.GetRequiredService<IWebHostEnvironment>();
      return new(Path.Combine(env.WebRootPath, options.Value.AssetPath));
    });

    services.AddZeroBackofficeUIComposition();

    //services.AddTransient<ISectionsApi, SectionsApi>();
    //services.AddTransient<ISettingsApi, SettingsApi>();

    //services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();


    PostConfigureServices?.Invoke(services, configuration);
  }


  protected void ConfigureOptions(BackofficeOptions options, IWebHostEnvironment env)
  {
    options.ExcludedPaths.Add("resources");
    options.AssetPath = "zero/resources";

    options.DevServer.WorkingDirectory = Path.Combine(env.ContentRootPath, "..", "Zero.Web.UI", "App");

    options.IconSets.Add(new BackofficeIconSet()
    {
      Alias = "feather",
      Name = "Feather",
      SpritePath = "icons/feather.svg",
      Prefix = "fth"
    });

    options.SupportedLanguages = new string[2] { "en-US", "de-DE" };
    options.DefaultLanguage = options.SupportedLanguages[0];
  }
}