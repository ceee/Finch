using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Stores;

public class StoresModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IStoreContext, StoreContext>();
    services.AddSingleton<IStoreCache, StoreCache>();

    services.AddOptions<FlavorOptions>();
  }
}