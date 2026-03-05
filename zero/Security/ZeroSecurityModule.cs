using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Security;

internal class ZeroSecurityModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    IConfigurationSection captchaSection = configuration.GetSection("Zero:Captcha");
    services.AddOptions<CaptchaOptions>().Bind(captchaSection);

    services.AddPowCapServer(options =>
    {
      CaptchaOptions captchaOptions = captchaSection.Get<CaptchaOptions>() ?? new CaptchaOptions();
      options.Default = captchaOptions;
    });
  }

  public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
  {
    IOptionsMonitor<CaptchaOptions> options = serviceProvider.GetRequiredService<IOptionsMonitor<CaptchaOptions>>();
    string endpointPrefix = options.CurrentValue.Endpoint;

    app.UseRouting();
    app.MapPowCapServer(endpointPrefix);
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapGet(endpointPrefix + "/cap.wasm", async context =>
      {
        Assembly assembly = typeof(ZeroSecurityModule).GetTypeInfo().Assembly;
        Stream resource = assembly.GetManifestResourceStream("zero.Resources.cap_wasm_bg_0_0_6.wasm");

        if (resource is null)
        {
          context.Response.StatusCode = StatusCodes.Status404NotFound;
          return;
        }

        await using (resource)
        {
          context.Response.ContentType = "application/wasm";
          await resource.CopyToAsync(context.Response.Body, context.RequestAborted);
        }
      });

      endpoints.MapGet(endpointPrefix + "/cap.widget.js", async context =>
      {
        Assembly assembly = typeof(ZeroSecurityModule).GetTypeInfo().Assembly;
        Stream resource = assembly.GetManifestResourceStream("zero.Resources.cap_min_0_1_41.js");

        if (resource is null)
        {
          context.Response.StatusCode = StatusCodes.Status404NotFound;
          return;
        }

        await using (resource)
        {
          context.Response.ContentType = "text/javascript";
          await resource.CopyToAsync(context.Response.Body, context.RequestAborted);
        }
      });
    });
    StaticCaptchaService.Configure(serviceProvider);
  }
}