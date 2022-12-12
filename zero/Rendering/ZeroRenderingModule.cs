using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Rendering;

public class ZeroRenderingModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IRazorRenderer, RazorRenderer>();
    
    services.AddOptions<IconOptions>().Bind(configuration.GetSection("Zero:Icons"));
  }
}