using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Architecture;

public abstract class ZeroPlugin : IZeroPlugin
{
  public IZeroPluginOptions Options { get; } = new ZeroPluginOptions();

  public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration) { }

  public virtual void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider) { }
}


public interface IZeroPlugin
{
  IZeroPluginOptions Options { get; }

  void ConfigureServices(IServiceCollection services, IConfiguration configuration);

  void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider);
}