using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PowCapServer;
using PowCapServer.Abstractions;

namespace Finch.Security;

internal class FinchSecurityModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    IConfigurationSection captchaSection = configuration.GetSection("Finch:Captcha");

    services.TryAddSingleton<ICaptchaService, DefaultCaptchaService>();
    services.TryAddSingleton<ICaptchaStore, DefaultCaptchaStore>();
    services.TryAddTransient<ISerializer, DefaultSerializer>();
    services.Configure<PowCapServerOptions>(opts => opts.Default = captchaSection.Get<CaptchaOptions>() ?? new CaptchaOptions());
    services.AddOptions<CaptchaOptions>().Bind(captchaSection);
  }

  public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
  {
    // if (app is WebApplication webApp)
    // {
    //   webApp.MapGet("/teamup/sync", async (ISchedulerFactory scheduler) =>
    //   {
    //     await SyncTeamUpDataJob.RunNow(scheduler);
    //     return "done";
    //   });
    // }

    StaticCaptchaService.Configure(serviceProvider);
  }
}