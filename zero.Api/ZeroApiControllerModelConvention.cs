using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text;

namespace zero.Api.Controllers;

public class ZeroApiControllerModelConvention : IControllerModelConvention
{
  readonly AttributeRouteModel RouteModel;

  readonly Type BaseClass = typeof(ZeroApiController);


  public ZeroApiControllerModelConvention(string zeroPath, string apiPath = "api", bool isAppAware = false)
  {
    StringBuilder path = new();
    path.Append(zeroPath.EnsureEndsWith('/'));
    path.Append(apiPath.TrimStart('/').EnsureEndsWith('/'));

    if (isAppAware)
    {
      path.Append("{zero_app_slug}/");
    }

    path.Append("[controller]");

    RouteModel = new AttributeRouteModel(new RouteAttribute(path.ToString()));
  }


  /// <summary>
  /// Configure routing model for all backoffice controllers
  /// </summary>
  public void Apply(ControllerModel controller)
  {
    bool hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);

    if (controller.ControllerType.IsSubclassOf(BaseClass))
    {
      controller.Selectors[0].AttributeRouteModel = RouteModel;
    }
  }
}