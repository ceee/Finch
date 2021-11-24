using Microsoft.Extensions.DependencyInjection;

namespace zero.Pages;

internal static class ServiceCollectionExtensions
{
  public static IServiceCollection AddZeroPages(this IServiceCollection services)
  {
    services.AddScoped<IPagesStore, PagesStore>();
    services.AddScoped<IPageTypeService, PageTypeService>();

    services.Configure<ZeroOptions>(opts =>
    {
      RavenOptions raven = opts.For<RavenOptions>();
      raven.Indexes.Add<Pages_AsHistory>();
      raven.Indexes.Add<Pages_ByHierarchy>();
      raven.Indexes.Add<Pages_ByType>();
      raven.Indexes.Add<Pages_WithChildren>();
    });
    return services;
  }
}