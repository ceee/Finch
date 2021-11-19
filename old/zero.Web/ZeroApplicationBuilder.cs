using Microsoft.AspNetCore.Builder;
using System;
using zero.Core.Options;
using zero.Web.Middlewares;

namespace zero.Web
{
  public class ZeroApplicationBuilder : IZeroApplicationBuilder
  {
    protected IApplicationBuilder App { get; private set; }

    protected IZeroOptions Options { get; private set; }


    internal ZeroApplicationBuilder(IApplicationBuilder app)
    {
      App = app;
      App.UseStaticFiles();
      App.UseMiddleware<ZeroContextMiddleware>();
      App.UseRouting();
      App.UseAuthentication();
      App.UseAuthorization();
    }


    public void WithEndpoints(Action<IZeroEndpointRouteBuilder> configure)
    {
      App.UseEndpoints(e =>
      {
        ZeroEndpointRouteBuilder builder = new(e);
        configure(builder);
      });
    }


    public IZeroApplicationBuilder WithMiddleware(Action<IApplicationBuilder> configure)
    {
      configure(App);
      return this;
    }
  }


  public interface IZeroApplicationBuilder
  {
    void WithEndpoints(Action<IZeroEndpointRouteBuilder> endpoints);

    IZeroApplicationBuilder WithMiddleware(Action<IApplicationBuilder> configure);
  }
}
