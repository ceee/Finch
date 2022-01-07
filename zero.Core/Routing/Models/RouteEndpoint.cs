using Microsoft.AspNetCore.Routing;

namespace zero.Routing;

public class ControllerRouteEndpoint : IRouteEndpoint
{
  public string Controller { get; set; }

  public string Action { get; set; }

  public ControllerRouteEndpoint(string controller, string action = "Index")
  {
    Controller = controller;
    Action = action;
  }

  public virtual void Apply(RouteValueDictionary values)
  {
    values["controller"] = Controller;
    values["action"] = Action;
  }
}


public class PageRouteEndpoint : IRouteEndpoint
{
  public string Page { get; set; }

  public PageRouteEndpoint(string page)
  {
    Page = page;
  }

  public virtual void Apply(RouteValueDictionary values)
  {
    values["page"] = Page;
  }
}


public interface IRouteEndpoint
{
  /// <summary>
  /// Append values to the RouteValueDictionary
  /// </summary>
  void Apply(RouteValueDictionary values);
}