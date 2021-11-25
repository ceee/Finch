using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.FileStorage;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroFileStorage(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<IPaths>(factory => new Paths(factory.GetService<IWebHostEnvironment>()));

    services.AddOptions<FileSystemOptions>().Bind(config.GetSection("Zero:FileSystem")).Configure(opts =>
    {
      opts.ZeroAssetsPath = "zero";
    });

    return services;
  }
}