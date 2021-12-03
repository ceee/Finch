using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using System.Text;

namespace zero.Api.Controllers;

public class ZeroApiControllerModelConvention : IControllerModelConvention
{
  readonly AttributeRouteModel AppAwareRouteModel;

  readonly AttributeRouteModel AppUnawareRouteModel;

  readonly Type BaseClass = typeof(ZeroApiController);

  readonly bool RuntimeIsAppAware = false;


  public ZeroApiControllerModelConvention(string zeroPath, string apiPath = "api", bool isAppAware = false)
  {
    RuntimeIsAppAware = isAppAware;
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
      bool isAppAware = RuntimeIsAppAware && controller.ControllerType.GetCustomAttribute<ZeroSystemApiAttribute>() == null;

      controller.Selectors[0].AttributeRouteModel = isAppAware ? AppAwareRouteModel : AppUnawareRouteModel;
    }
  }


  protected AttributeRouteModel BuildRouteModel(string zeroPath, string apiPath, bool appAware = false)
  {
    StringBuilder path = new();
    path.Append(zeroPath.EnsureEndsWith('/'));
    path.Append(apiPath.TrimStart('/').EnsureEndsWith('/'));

    if (appAware)
    {
      path.Append("{zero_app_slug}/");
    }

    path.Append("[controller]");

    return new AttributeRouteModel(new RouteAttribute(path.ToString()));
  }
}