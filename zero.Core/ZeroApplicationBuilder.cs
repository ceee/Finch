using Microsoft.AspNetCore.Builder;

namespace zero;

public class ZeroApplicationBuilder : IZeroApplicationBuilder
{
  protected IApplicationBuilder App { get; private set; }

  protected IZeroOptions Options { get; private set; }


  internal ZeroApplicationBuilder(IApplicationBuilder app)
  {
    App = app;
    App.UseStaticFiles();
    //App.UseExceptionHandler("/zero/api/error");
    App.UseMiddleware<ZeroContextMiddleware>();
    App.UseRouting();
    App.UseAuthentication();
    App.UseAuthorization();

    ZeroBuilder.Modules.Configure(app, null, app.ApplicationServices);

    foreach (IZeroPlugin plugin in ZeroBuilder.Plugins)
    {
      plugin.Configure(app, null, app.ApplicationServices);
    }


    //ZeroModuleInitializer.ConfigureAll(app.ApplicationServices.GetRequiredService<IZeroOptions>());
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