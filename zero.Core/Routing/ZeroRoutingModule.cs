using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace zero.Routing;

internal class ZeroRoutingModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IRequestUrlResolver, RequestUrlResolver>();
    services.AddScoped<IRoutes, Routes>();
    services.AddScoped<IRouteResolver, RouteResolver>();
    services.AddScoped<IRedirectAutomation, RedirectAutomation>();
    services.AddScoped<IPageUrlBuilder, PageUrlBuilder>();
    services.AddScoped<IRouteProvider, PageRouteProvider>();
    services.AddScoped<ILinks, Links>();
    services.AddScoped<ILinkProvider, PageLinkProvider>();
    services.AddScoped<ILinkProvider, RawLinkProvider>();
    services.AddTransient<ZeroRoutesTransformer>();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, NotFoundSelectorPolicy>());
    //services.AddScoped<IInterceptor, ZeroEntityRouteInterceptor>();

    services.AddOptions<RoutingOptions>().Bind(configuration.GetSection("Zero:Routing"));

    services.Configure<ZeroOptions>(opts =>
    {
      RavenOptions raven = opts.For<RavenOptions>();
      raven.Indexes.Add<RouteRedirects_ByUrl>();
      raven.Indexes.Add<Routes_ByDependencies>();
      raven.Indexes.Add<Routes_ForResolver>();
    });
  }
}