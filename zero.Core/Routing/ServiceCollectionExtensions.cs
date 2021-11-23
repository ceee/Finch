using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace zero.Routing;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroRouting(this IServiceCollection services)
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
    services.AddScoped<ZeroRoutesTransformer>();
    services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, NotFoundSelectorPolicy>());
    services.AddScoped<IInterceptor, ZeroEntityRouteInterceptor>();

    services.Configure<ZeroOptions>(opts =>
    {
      RavenOptions raven = opts.For<RavenOptions>();
      raven.Indexes.Add<RouteRedirects_ByUrl>();
      raven.Indexes.Add<Routes_ByDependencies>();
      raven.Indexes.Add<Routes_ForResolver>();
    });

    return services;
  }
}