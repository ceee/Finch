namespace zero.Routing;

public class Routes_ByDependencies : ZeroIndex<Route>
{
  protected override void Create()
  {
    Map = items => items
      .Select(x => new
      {
        Dependencies = x.Dependencies
      });
  }
}