using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Configuration;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroConfiguration(this IServiceCollection services, IConfiguration config)
  {
    services.AddOptions<ZeroOptions>().Configure<IServiceProvider>((opts, svc) =>
    {
      opts.ServiceProvider = svc;
      opts.Version = "0.0.1-alpha.1";
      opts.ZeroPath = "/zero";
      opts.TokenExpiration = TimeSpan.FromHours(3); 
    }).Bind(config.GetSection("Zero"));
    services.AddTransient<IZeroOptions, ZeroOptions>(factory => factory.GetService<IOptions<ZeroOptions>>().Value);

    services.AddOptions<FeatureOptions>().Bind(config.GetSection("Zero:Features"));

    return services;
  }
}