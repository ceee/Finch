using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace zero.Routing;

public class ZeroEndpointRouteBuilder : IZeroEndpointRouteBuilder
{
  readonly IEndpointRouteBuilder _builder;

  public IServiceProvider ServiceProvider => _builder.ServiceProvider;

  public ICollection<EndpointDataSource> DataSources => _builder.DataSources;

  internal ZeroEndpointRouteBuilder(IEndpointRouteBuilder builder)
  {
    _builder = builder;
  }

  public IApplicationBuilder CreateApplicationBuilder() => _builder.CreateApplicationBuilder();
}


public interface IZeroEndpointRouteBuilder : IEndpointRouteBuilder
{

}