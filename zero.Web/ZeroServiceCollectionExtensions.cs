using Microsoft.Extensions.DependencyInjection;
using System;
using zero.Core;
using zero.Web.Sections;

namespace zero.Web
{
  public static class ZeroServiceCollectionExtensions
  {
    public static ZeroBuilder AddZero(this IServiceCollection services)
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

      return new ZeroBuilder(services);
    }

    public static ZeroBuilder AddZero(this IServiceCollection services, Action<ZeroOptions> setupAction)
    {
      ZeroBuilder builder = services.AddZero();
      builder.Services.PostConfigure(setupAction);
      return builder;
    }
  }
}
