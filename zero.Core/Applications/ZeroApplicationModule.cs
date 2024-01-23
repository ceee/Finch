using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

internal class ZeroApplicationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IApplicationResolver, ApplicationResolver>();
    services.AddScoped<IApplicationRegistry, ApplicationRegistry>();
    //services.AddOptions<ApplicationOptions>().Bind(configuration.GetSection("Zero:Applications"));
  }
}