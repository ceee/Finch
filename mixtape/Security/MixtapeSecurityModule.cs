using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PowCapServer;
using PowCapServer.Abstractions;

namespace Mixtape.Security;

internal class MixtapeSecurityModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    IConfigurationSection captchaSection = configuration.GetSection("Mixtape:Captcha");

    services.TryAddSingleton<ICaptchaService, DefaultCaptchaService>();
    services.TryAddSingleton<ICaptchaStore, DefaultCaptchaStore>();
    services.TryAddTransient<ISerializer, DefaultSerializer>();
    services.Configure<PowCapServerOptions>(opts => opts.Default = captchaSection.Get<CaptchaOptions>() ?? new CaptchaOptions());
    services.AddOptions<CaptchaOptions>().Bind(captchaSection);

    services.AddDataProtection();
    services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(s =>
    {
      ILoggerFactory loggerFactory = s.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
      MixtapeOptions mixtapeOptions = s.GetService<IOptions<MixtapeOptions>>().Value;
      string contentRootPath = s.GetService<IHostEnvironment>().ContentRootPath;

      return new ConfigureOptions<KeyManagementOptions>(options =>
      {
        string keyPath = Path.GetFullPath(Path.Combine(contentRootPath, mixtapeOptions.DataProtectionStoragePath));
        options.XmlRepository = new FileSystemXmlRepository(new DirectoryInfo(keyPath), loggerFactory);
      });
    });
  }

  public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
  {
    StaticCaptchaService.Configure(serviceProvider);
  }
}