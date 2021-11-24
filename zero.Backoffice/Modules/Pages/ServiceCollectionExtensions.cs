using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.Modules;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroBackofficePages(this IServiceCollection services)
  {
    services.AddScoped<IPageTreeService, PageTreeService>();
    return services;
  }
}