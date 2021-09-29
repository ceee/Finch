using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace zero.Web
{
  public class ZeroEndpointRouteBuilder : IZeroEndpointRouteBuilder
  {
    IEndpointRouteBuilder _builder;

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
}
