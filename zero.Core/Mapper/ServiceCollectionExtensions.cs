using Microsoft.Extensions.DependencyInjection;

namespace zero.Mapper;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroMapper(this IServiceCollection services)
  {
    services.AddScoped<IZeroMapper, ZeroMapper>();
    return services;
  }
}