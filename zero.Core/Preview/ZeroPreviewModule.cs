using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Preview;

internal class ZeroPreviewModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IPreviewService, PreviewService>();
    services.AddOptions<PreviewOptions>().Bind(configuration.GetSection("Zero:Preview"));
  }
}