using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Stores;

internal class ZeroStoreModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<StoreContext>();
    services.AddTransient<IStoreOperations, StoreOperations>();

    services.AddOptions<FlavorOptions>();

    services.ConfigureOptions<ConfigureFlavorJsonOptions>();
  }
}