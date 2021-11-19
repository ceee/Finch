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
    config.Services.AddScoped<ZeroEntityRouteInterceptor>();
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    options.Raven.Indexes.Add<RouteRedirects_ByUrl>();
    options.Raven.Indexes.Add<Routes_ByDependencies>();
    options.Raven.Indexes.Add<Routes_ForResolver>();
    options.Interceptors.Add<ZeroEntityRouteInterceptor, ZeroEntity>(gravity: 100);
  }
}