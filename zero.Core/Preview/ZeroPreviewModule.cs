using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace zero.Preview;

internal class ZeroPreviewModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    //services.AddScoped<IRouteProvider, PreviewRouteProvider>();
    services.AddOptions<PreviewOptions>().Bind(configuration.GetSection("Zero:Preview"));
  }
}