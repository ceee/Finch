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
    options.For<RavenOptions>().Indexes.Add<zero_Backoffice_Countries_Listing>();
  }
}