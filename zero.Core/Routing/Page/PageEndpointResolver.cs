namespace zero.Core.Routing
{
  public class PageEndpointResolver : IPageEndpointResolver
  {
    protected RouteProviderEndpoint DefaultEndpoint { get; set; }


    public PageEndpointResolver()
    {
      DefaultEndpoint = new RouteProviderEndpoint()
      {
        Controller = "DefaultRoute",
        Action = "Index"
      };
    }


    public virtual RouteProviderEndpoint GetEndpoint(PageRoute route)
    {
      return DefaultEndpoint;
    }
  }
}
