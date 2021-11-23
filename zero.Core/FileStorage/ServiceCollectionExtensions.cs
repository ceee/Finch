using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace zero.FileStorage;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroFileStorage(this IServiceCollection services)
  {
    services.AddScoped<IPaths>(factory => new Paths(factory.GetService<IWebHostEnvironment>(), true));
    return services;
  }
}