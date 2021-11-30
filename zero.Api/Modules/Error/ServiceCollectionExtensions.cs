using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Modules.Error;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBackofficeErrorHandler(this IServiceCollection services)
  {
    return services;
  }
}