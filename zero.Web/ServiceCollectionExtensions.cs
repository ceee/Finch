using Microsoft.Extensions.DependencyInjection;
using System;
using zero.Core;
using zero.Web.Sections;

namespace zero.Web
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddZero(this IServiceCollection services)
    {
      services.AddOptions<ZeroOptions>()
        .Configure(opts =>
        {
          opts.Sections.Add<DashboardSection>();
          opts.Sections.Add<PagesSection>();
          opts.Sections.Add<ListsSection>();
          opts.Sections.Add<MediaSection>();
          opts.Sections.Add<SettingsSection>();
        });

      return services;
    }

    public static IServiceCollection AddZero(this IServiceCollection services, Action<ZeroOptions> setupAction)
    {
      return services.AddZero().PostConfigure(setupAction);
    }
  }
}
