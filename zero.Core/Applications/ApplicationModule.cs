using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Applications;

public class ApplicationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IApplicationResolver, ApplicationResolver>();
    services.AddScoped<IApplicationStore, ApplicationStore>();
    services.AddOptions<ApplicationOptions>().Bind(configuration.GetSection("Zero:Applications"));
  }
}