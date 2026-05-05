using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Rendering;

public class MixtapeRenderingModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IRazorRenderer, RazorRenderer>();
    
    services.AddOptions<IconOptions>().Bind(configuration.GetSection("Mixtape:Icons"));
  }
}