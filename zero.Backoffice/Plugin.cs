using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace zero.Backoffice;

internal class ZeroBackofficePlugin : ZeroPlugin
{
  public ZeroBackofficePlugin()
  {
    Options.Name = "zero.Defaults";
    Options.LocalizationPaths.Add("~/Resources/Localization/zero.{lang}.json");
  }


  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<BackofficeOptions>().Bind(configuration.GetSection("Zero:Backoffice")).Configure(ConfigureOptions);
    services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, ZeroBackofficeMvcOptions>());

    // register all modules
    ZeroModuleConfiguration moduleConfig = new(services, configuration);
    foreach (ZeroModule module in Registrations.Modules)
    {
      module.Register(moduleConfig);
    }
  }


  protected void ConfigureOptions(BackofficeOptions options)
  {
    options.Path = "/zero";
    options.IconSets.Add(new BackofficeIconSet()
    {
      Alias = "feather",
      Name = "Feather",
      SpritePath = "/assets/icons/feather.svg",
      Prefix = "fth"
    });

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