using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Validation;

internal class ZeroValidationModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddValidationLanguageExtensions();
    services.AddScoped(typeof(IZeroMergedValidator<>), typeof(ZeroMergedValidator<>));
  }
}