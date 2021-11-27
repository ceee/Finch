using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.Modules;

internal class TranslationsModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddOptions<SearchOptions>().Bind(config.Configuration.GetSection(SearchOptions.KEY));
    config.Services.AddScoped<IBackofficeSearchService, BackofficeSearchService>();
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    options.For<RavenOptions>().Indexes.Add<zero_Backoffice_Search>();
  }
}