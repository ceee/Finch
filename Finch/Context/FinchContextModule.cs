using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Context;

internal class FinchContextModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IFinchContext, FinchContext>();
    services.AddHttpContextAccessor();
  }
}