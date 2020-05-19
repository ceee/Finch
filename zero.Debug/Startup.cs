using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using zero.Commerce.Backoffice;
using zero.Commerce.Entities;
using zero.Core.Renderer;
using zero.Debug.Models;
using zero.TestData;
using zero.Web;

namespace zero.Debug
{
  public class Startup
  {
    IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }



    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      }).AddCookie(options =>
      {
        options.LoginPath = "/Account/Index";
      });

      ZeroBuilder zero = services.AddZero(Configuration);
      zero.AddPlugin<TestPlugin>();
      zero.AddPlugin<Commerce.CommercePlugin>();

      zero.WithOptions(opts =>
      {
        opts.Extend<IChannel, SalesChannel>();

        //IRenderer<IChannel> renderer = (IRenderer<IChannel>)new SalesChannelRenderer();
        //renderer.
        //opts.Renderers.Extend<IChannel, SalesChannelRenderer>();
      });

      services.AddMvc();

      services.Configure<IISOptions>(opts => opts.AutomaticAuthentication = false);
    }


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseZeroDevEnvironment();
      }

      app.UseRouting();
      app.UseAuthentication();

      app.UseZero();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}
