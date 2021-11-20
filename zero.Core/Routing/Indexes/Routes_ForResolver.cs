namespace zero.Routing;

public class Routes_ForResolver : ZeroIndex<Route>
{
  protected override void Create()
  {
    Map = items => items
      .Select(x => new
      {
        Url = x.Url,
        AllowSuffix = x.AllowSuffix
      });
  }
}