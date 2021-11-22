using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal class ArchitectureModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IBlueprintService, BlueprintService>();   
    config.Services.AddScoped<IInterceptor, BlueprintInterceptor>();
    config.Services.AddScoped<IInterceptor, BlueprintChildInterceptor>();
  }
}