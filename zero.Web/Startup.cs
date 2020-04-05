using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Extensions;
using zero.Web.Sections;

namespace zero.Web
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
      services.Configure<IZeroConfiguration>(config);
      ZeroConfiguration appConfig = new ZeroConfiguration();
      ConfigurationBinder.Bind(config, appConfig);
      services.AddSingleton<IZeroConfiguration>(appConfig);

      // add zero core
      //services.AddCore(appConfig, env);
      services.AddZero(opts =>
      {
        //opts.Sections.RemoveAt(1);
        //var section = new Section("ecommerce", "E-Commerce", "shopping-bag");
        //section.Children.Add(new Section("analytics", "Analytics"));
        //opts.Sections.Add(section);
      });

      // add cookie-based authentication
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(opts => {
          // override redirect to login page (handled by vue frontend) and return a 401 instead
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

      // TODO move registration into core
      services.AddTransient<ISetupApi, SetupApi>();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<ZeroOptions> zeroOptions)
    {
      string zeroPath = zeroOptions.CurrentValue.BackofficePath.EnsureEndsWith('/');

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
          pattern: zeroPath + "api/{controller=Index}/{action=Index}/{id?}"
        );

        // fallbacks for SPA
        endpoints.MapFallbackToController(zeroPath + "{**path}", "Index", "Index");
      });
    }
  }
}
