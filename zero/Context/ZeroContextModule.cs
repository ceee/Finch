using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Context;

internal class ZeroContextModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IZeroContext, ZeroContext>();
    services.AddHttpContextAccessor();
  }
}