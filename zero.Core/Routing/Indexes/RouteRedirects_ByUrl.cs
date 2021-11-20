namespace zero.Routing;

public class RouteRedirects_ByUrl : ZeroIndex<RouteRedirect>
{
  protected override void Create()
  {
    Map = items => items
      .Select(x => new
      {
        IsAutomated = x.IsAutomated,
        SourceUrl = x.SourceUrl,
        TargetUrl = x.TargetUrl
      });
  }
}