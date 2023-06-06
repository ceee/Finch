using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Numbers;

internal class ZeroNumberModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddOptions<NumberOptions>().Bind(configuration.GetSection("Zero:Numbers"));

    services.AddScoped<INumbers, Numbers>();
    services.AddScoped<IValidator<Number>, NumberValidator>();
    services.AddScoped<IZeroNumberStoreDbProvider, EmptyZeroNumberStoreDbProvider>();
  }
}