using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal class ContextModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IZeroContext, ZeroContext>();
    config.Services.AddHttpContextAccessor();
  }
}