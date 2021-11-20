namespace zero.Pages;

internal class PagesModule : ZeroModule
{
  /// <inheritdoc />
  public override void Configure(IZeroOptions options)
  {
    options.Raven.Indexes.Add<Pages_AsHistory>();
    options.Raven.Indexes.Add<Pages_ByHierarchy>();
    options.Raven.Indexes.Add<Pages_ByType>();
    options.Raven.Indexes.Add<Pages_WithChildren>();
  }
}