using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finch.Mvc;

public class FinchMvcModule : FinchModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddRouting(opts => opts.LowercaseUrls = true);
    services.AddHealthChecks();
  }


  public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
  {
    routes.MapHealthChecks("/health");
  }
}