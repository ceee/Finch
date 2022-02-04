using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Search;

internal class ZeroSearchModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<ISearchService, SearchService>();

    services.Configure<RavenOptions>(opts =>
    {
      opts.Indexes.Add<zero_Search>();
    });

    services.AddOptions<ZeroSearchOptions>().Bind(configuration.GetSection("Zero:Search"));
  }
}