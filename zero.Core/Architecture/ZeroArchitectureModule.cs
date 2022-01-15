using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Architecture;

internal class ZeroArchitectureModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IBlueprintService, BlueprintService>();
    //services.AddScoped<IInterceptor, BlueprintInterceptor>();
    //services.AddScoped<IInterceptor, BlueprintChildInterceptor>();
    services.AddOptions<BlueprintOptions>().Bind(configuration.GetSection("Zero:Blueprints"));
  }
}