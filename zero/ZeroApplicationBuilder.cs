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
  }

  public IZeroApplicationBuilder WithMiddleware(Action<IApplicationBuilder> configure)
  {
    configure(App);
    return this;
  }
}


public interface IZeroApplicationBuilder
{
  IZeroApplicationBuilder WithMiddleware(Action<IApplicationBuilder> configure);
}