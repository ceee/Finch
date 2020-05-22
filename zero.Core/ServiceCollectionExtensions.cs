using Microsoft.Extensions.DependencyInjection;
using zero.Core.Api;

namespace zero.Core.Entities
{
  public static class ServiceCollectionExtensions
  {
    public static void AddZeroCoreServices(this IServiceCollection services)
    {
      services.AddTransient<IApplication, Application>();
      services.AddTransient(typeof(IApplicationsApi<>), typeof(ApplicationsApi<>));
    }
  }
}
