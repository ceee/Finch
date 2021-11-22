using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace zero.Routing;

internal class RoutingModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IRequestUrlResolver, RequestUrlResolver>();
    config.Services.AddScoped<IRoutes, Routes>();
    config.Services.AddScoped<IRouteResolver, RouteResolver>();
    config.Services.AddScoped<IRedirectAutomation, RedirectAutomation>();
    config.Services.AddScoped<IPageUrlBuilder, PageUrlBuilder>();
    config.Services.AddScoped<IRouteProvider, PageRouteProvider>();
    config.Services.AddScoped<ILinks, Links>();
    config.Services.AddScoped<ILinkProvider, PageLinkProvider>();
    config.Services.AddScoped<ILinkProvider, RawLinkProvider>();
    config.Services.AddScoped<ZeroRoutesTransformer>();
    config.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, NotFoundSelectorPolicy>());
    config.Services.AddScoped<IInterceptor, ZeroEntityRouteInterceptor>();
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    RavenOptions raven = options.For<RavenOptions>();
    InterceptorOptions interceptors = options.For<InterceptorOptions>();

    raven.Indexes.Add<RouteRedirects_ByUrl>();
    raven.Indexes.Add<Routes_ByDependencies>();
    raven.Indexes.Add<Routes_ForResolver>();
  }
}