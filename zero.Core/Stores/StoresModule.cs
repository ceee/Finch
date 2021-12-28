using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Stores;

public class StoresModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IStoreContext, StoreContext>();
    services.AddSingleton<IStoreCache, StoreCache>();

    services.AddTransient<IStoreOperations, StoreOperations>();
    services.AddTransient<ISharedStoreOperations, StoreOperations>(x => new StoreOperations(x.GetRequiredService<IStoreContext>(), new() { Database = x.GetRequiredService<IZeroOptions>().For<RavenOptions>().Database }));
    services.AddTransient<IStoreOperationsWithInactive, StoreOperations>(x => new StoreOperations(x.GetRequiredService<IStoreContext>(), new() { IncludeInactive = true }));
    services.AddTransient<ISharedStoreOperationsWithInactive, StoreOperations>(x => new StoreOperations(x.GetRequiredService<IStoreContext>(), new() { Database = x.GetRequiredService<IZeroOptions>().For<RavenOptions>().Database, IncludeInactive = true }));

    services.AddOptions<FlavorOptions>();

    services.ConfigureOptions<ConfigureFlavorJsonOptions>();
  }
}