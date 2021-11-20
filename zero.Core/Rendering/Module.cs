using Microsoft.Extensions.DependencyInjection;

namespace zero.Rendering;

internal class RenderingModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IRazorRenderer, RazorRenderer>();
  }
}