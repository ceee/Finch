using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice.Modules;

internal class CountriesModule : ZeroModule
{
  /// <inheritdoc />
  public override void Register(IZeroModuleConfiguration config)
  {
    
  }


  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    options.Raven.Indexes.Add<Countries_Listing>();
  }
}