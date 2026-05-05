using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mixtape.Context;

internal class MixtapeContextModule : MixtapeModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IMixtapeContext, MixtapeContext>();
    services.AddHttpContextAccessor();
  }
}