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
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using System;
using System.Threading.Tasks;
using zero.Core;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;

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

      // add raven
      services.AddSingleton(_ =>
      {
        DocumentStore store = new DocumentStore()
        {
          Urls = new string[1] { appConfig.Raven.Url },
          Database = appConfig.Raven.Database
        };

        store.Conventions.FindCollectionName = type =>
        {
          return Constants.Database.CollectionPrefix + DocumentConventions.DefaultGetCollectionName(type);
        };

        store.Conventions.IdentityPartsSeparator = ".";

        return store.Initialize();
      });
      

      // add zero core
      //services.AddCore(appConfig, env);
      services.AddZero(opts =>
      {
        //opts.Sections.RemoveAt(1);
        var section = new Section("commerce", "Commerce", "fth-shopping-bag", "#52bba1");
        section.Children.Add(new Section("orders", "Orders"));
        section.Children.Add(new Section("customers", "Customers"));
        section.Children.Add(new Section("catalogue", "Catalogue"));
        section.Children.Add(new Section("promotions", "Promotions"));
        section.Children.Add(new Section("channels", "Channels"));
        opts.Sections.Insert(3, section);
      });

      //services.AddAuthentication(opts =>
      //{
      //  opts.
      //});

      // add cookie-based authentication
      services.AddAuthentication(Constants.Auth.Scheme)
        .AddCookie(Constants.Auth.Scheme, opts => {
          opts.Cookie.Name = Constants.Auth.CookieName;
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
      services.AddTransient<ISectionsApi, SectionsApi>();
      services.AddTransient<IApplicationsApi, ApplicationsApi>();
      services.AddTransient<IPagesApi, PagesApi>();
      services.AddTransient<IPageTreeApi, PageTreeApi>();
      services.AddTransient<ISettingsApi, SettingsApi>();
      services.AddTransient<IAuthenticationApi, AuthenticationApi>();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<ZeroOptions> zeroOptions)
    {
      string zeroPath = zeroOptions.CurrentValue.BackofficePath.EnsureEndsWith('/').EnsureStartsWith('/');

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
      //app.UseCors();

      app.UseWhen(ctx => ctx.Request.Path.ToString().StartsWith(zeroPath.TrimEnd('/')), zeroApp =>
      {
        zeroApp.UseRouting();
        zeroApp.UseAuthentication();
        zeroApp.UseAuthorization();

        zeroApp.UseEndpoints(endpoints =>
        {
          // setup route
          endpoints.MapControllerRoute(
            name: "setup",
            pattern: zeroPath + "setup",
            defaults: new
            {
              controller = "Setup",
              action = "Index"
            }
          );

          // routes for API
          endpoints.MapControllerRoute(
            name: "api",
            pattern: zeroPath + "api/{controller=Index}/{action=Index}/{id?}"
          );

          // fallbacks for SPA
          endpoints.MapFallbackToController(zeroPath + "{**path}", "Index", "Index");
        });
      });
    }
  }
}
