using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Rendering;

public class FinchRenderingModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IRazorRenderer, RazorRenderer>();
    
    services.AddOptions<IconOptions>().Bind(configuration.GetSection("Finch:Icons"));
  }
}