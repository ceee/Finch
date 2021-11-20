namespace zero.Routing;

public class PageRoute : BasePageRoute
{
  public PageRoute() { }
  public PageRoute(Route route) : base(route) { }

  public IList<Page> Parents { get; set; }

  public string PageType { get; set; }
}