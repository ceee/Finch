using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.Modules;

internal class CountriesModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    config.Services.AddScoped<IPermissionProvider, CountryPermissions>();
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    options.For<RavenOptions>().Indexes.Add<zero_Backoffice_Countries_Listing>();
  }
}