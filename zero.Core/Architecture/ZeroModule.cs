using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Architecture;

public abstract class ZeroModule : IZeroModule
{
  /// <inheritdoc />
  public virtual int Order { get; } = 0;

  /// <inheritdoc />
  public virtual int ConfigureOrder => Order;

  /// <inheritdoc />
  public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration) { }

  /// <inheritdoc />
  public virtual void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider) { }
}


public interface IZeroModule
{
  /// <summary>
  /// Get the value to use to order startups to configure services. The default is 0.
  /// </summary>
  int Order { get; }

  /// <summary>
  /// Get the value to use to order startups to build the pipeline. The default is the 'Order' property.
  /// </summary>
  int ConfigureOrder { get; }

  /// <summary>
  /// This method gets called by the runtime. Use this method to add services to the container.
  /// For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
  /// </summary>
  /// <param name="services">The collection of service descriptors.</param>
  void ConfigureServices(IServiceCollection services, IConfiguration configuration);

  /// <summary>
  /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  /// </summary>
  /// <param name="builder"></param>
  /// <param name="routes"></param>
  /// <param name="serviceProvider"></param>
  void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider);
}