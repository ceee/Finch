using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Finch.Configuration;

internal class FinchConfigurationModule : FinchModule
{
  public override int Order => -1000;

  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<FinchOptions>().Configure<IServiceProvider>((opts, svc) =>
    {
      opts.ServiceProvider = svc;
      opts.Version = "1.0.0-alpha.1";
      opts.TokenExpiration = TimeSpan.FromHours(3);
      opts.DataProtectionStoragePath = "../cache/dpkeys";
    }).Bind(configuration.GetSection("Finch"));

    services.AddTransient<IFinchOptions, FinchOptions>(factory => factory.GetService<IOptions<FinchOptions>>().Value);
  }
}