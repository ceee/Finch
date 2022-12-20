using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Commerce.Numbers;

internal class ZeroCommerceNumberModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<INumberStore, NumberStore>();
    services.AddScoped<INumberService, NumberService>();
    services.AddSingleton<IPermissionProvider, NumberPermissions>();
    services.AddScoped<IInterceptor, NumberInterceptor>();
    services.AddScoped<IValidator<Number>, NumberValidator>();

    services.Configure<FlavorOptions>(opts =>
    {
      opts.Configure<Number>(cfg =>
      {
        cfg.CanUseWithoutFlavors = false;
      });
    });
  }
}