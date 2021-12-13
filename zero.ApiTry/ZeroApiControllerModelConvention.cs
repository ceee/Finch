using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using System.Text;

namespace zero.ApiTry;

public class ZeroApiControllerModelConvention : IControllerModelConvention
{
  readonly AttributeRouteModel AppAwareRouteModel;

  readonly AttributeRouteModel AppUnawareRouteModel;

  readonly Type BaseClass = typeof(AppApiController);


  public ZeroApiControllerModelConvention(string zeroPath, string apiPath = "api")
  {
    AppAwareRouteModel = BuildRouteModel(zeroPath, apiPath, true);
    AppUnawareRouteModel = BuildRouteModel(zeroPath, apiPath, false);
  }


  /// <summary>
  /// Configure routing model for all backoffice controllers
  /// </summary>
  public virtual void Apply(ControllerModel controller)
  {
    bool hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);

    if (controller.ControllerType.IsSubclassOf(BaseClass))
    {
      controller.Selectors[0].AttributeRouteModel = AppUnawareRouteModel;
    }
  }


  protected AttributeRouteModel BuildRouteModel(string zeroPath, string apiPath, bool appAware = false)
  {
    StringBuilder path = new();
    path.Append(zeroPath + '/');
    path.Append(apiPath.TrimStart('/').EnsureEndsWith('/'));

    if (appAware)
    {
      path.Append("{zero_app_slug}/");
    }

    path.Append("[controller]");

    return new AttributeRouteModel(new RouteAttribute(path.ToString()));
  }
}