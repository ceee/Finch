using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Validation;

internal class FinchValidationModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddValidationLanguageExtensions();
    services.AddScoped(typeof(IFinchMergedValidator<>), typeof(FinchMergedValidator<>));
  }
}