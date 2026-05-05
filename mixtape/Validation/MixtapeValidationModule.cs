using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Validation;

internal class MixtapeValidationModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddValidationLanguageExtensions();
    services.AddScoped(typeof(IMixtapeMergedValidator<>), typeof(MixtapeMergedValidator<>));
  }
}