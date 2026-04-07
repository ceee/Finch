using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Numbers;

internal class FinchNumberModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<NumberOptions>().Bind(configuration.GetSection("Finch:Numbers"));

    services.AddScoped<INumbers, Numbers>();
    services.AddScoped<IValidator<Number>, NumberValidator>();
    services.AddScoped<IFinchNumberStoreDbProvider, EmptyFinchNumberStoreDbProvider>();
  }
}