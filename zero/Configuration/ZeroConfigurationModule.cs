using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Configuration;

internal class ZeroConfigurationModule : ZeroModule
{
  public override int Order => -1000;

  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<ZeroOptions>().Configure<IServiceProvider>((opts, svc) =>
    {
      opts.ServiceProvider = svc;
      opts.Version = "1.0.0-alpha.1";
      opts.TokenExpiration = TimeSpan.FromHours(3);
    }).Bind(configuration.GetSection("Zero"));

    services.AddTransient<IZeroOptions, ZeroOptions>(factory => factory.GetService<IOptions<ZeroOptions>>().Value);
  }
}