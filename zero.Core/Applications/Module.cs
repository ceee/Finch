using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal class ApplicationsModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IApplicationResolver, ApplicationResolver>();
  }
}