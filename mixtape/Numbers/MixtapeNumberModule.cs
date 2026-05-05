using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Numbers;

internal class MixtapeNumberModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<NumberOptions>().Bind(configuration.GetSection("Mixtape:Numbers"));

    services.AddScoped<INumbers, Numbers>();
    services.AddScoped<IValidator<Number>, NumberValidator>();
    services.AddScoped<IMixtapeNumberStoreDbProvider, EmptyMixtapeNumberStoreDbProvider>();
  }
}