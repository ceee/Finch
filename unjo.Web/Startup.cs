using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using unjo.Core;

namespace unjo.Web
{
  public class Startup
  {
    private readonly IConfiguration config;

    private IWebHostEnvironment env;


    /// <summary>
    /// Bootstrap the application
    /// </summary>
    public Startup(IConfiguration config, IWebHostEnvironment env)
    {
      this.config = config;
      this.env = env;
    }


    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
      //CultureInfo cultureInfo = new CultureInfo("en-US");
      //cultureInfo.NumberFormat.CurrencySymbol = "€";
      //CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

      // build and register app configuration
      services.Configure<IBackofficeConfiguration>(config);
      BackofficeConfiguration appConfig = new BackofficeConfiguration();
      ConfigurationBinder.Bind(config, appConfig);
      services.AddSingleton<IBackofficeConfiguration>(appConfig);

      // add unjo core
      //services.AddCore(appConfig, env);

      // add cookie-based authentication
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(opts => {
          opts.Events.OnRedirectToLogin = (context) =>
          {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
          };
        });

      // add Core MVC
      IMvcBuilder mvc = services.AddMvc(opts =>
      {
        opts.Filters.Add<Core.Attributes.OperationCancelledExceptionFilterAttribute>();
      })
      //.ExtendWithCore()
      .AddNewtonsoftJson(opts =>
      {
        opts.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'" });
        opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
        opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      });

      if (Environment.GetEnvironmentVariable("DOTNET_WATCH") == "1")
      {
        mvc.AddRazorRuntimeCompilation();
      }

      services.Configure<IISOptions>(options =>
      {
        options.AutomaticAuthentication = false;
      });

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }


    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // enable webpack middleware
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        //app.UseDeveloperExceptionPage();
        #pragma warning disable CS0618
        app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions()
        {
          HotModuleReplacement = true
        });
        #pragma warning restore CS0618
      }
      else
      {
        app.UseHsts();
      }

      app.UseStaticFiles();
      app.UseRouting();
      //app.UseCors();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        // routes for API
        endpoints.MapControllerRoute(
          name: "api",
          pattern: "api/{controller=Index}/{action=Index}/{id?}"
        );

        // default routes
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Index}/{action=Index}/{id?}"
        );

        // fallbacks for SPA
        endpoints.MapFallbackToController("Index", "Index");
      });
    }
  }
}
