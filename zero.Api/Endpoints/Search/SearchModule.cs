using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace zero.Api.Endpoints.Search;

public class SearchModule : ZeroModule
{
  public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
  {
    services.AddSingleton<IPermissionProvider, SearchPermissions>();
    services.AddScoped<ISearchService, SearchService>();

    //services.Configure<RavenOptions>(opts =>
    //{
    //  opts.Indexes.Add<zero_Backoffice_Search>();
    //});

    services.AddOptions<SearchOptions>().Bind(configuration.GetSection("Zero:Search"));
  }
}