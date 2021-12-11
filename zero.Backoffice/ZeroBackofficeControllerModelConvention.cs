using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using zero.Api.Controllers;
using zero.Api.Filters;

namespace zero.Backoffice;

public class ZeroBackofficeControllerModelConvention : ZeroApiControllerModelConvention
{
  readonly AttributeRouteModel AppAwareRouteModel;

  readonly AttributeRouteModel AppUnawareRouteModel;

  readonly Type BaseClass = typeof(ZeroBackofficeController);

  readonly bool RuntimeIsAppAware = false;


  public ZeroBackofficeControllerModelConvention(string zeroPath, string backofficeApiPath = "backoffice", bool isAppAware = false) : base(zeroPath, backofficeApiPath, isAppAware)
  {
    RuntimeIsAppAware = isAppAware;
    AppAwareRouteModel = BuildRouteModel(zeroPath, backofficeApiPath, true);
    AppUnawareRouteModel = BuildRouteModel(zeroPath, backofficeApiPath, false);
  }


  /// <summary>
  /// Configure routing model for all backoffice controllers
  /// </summary>
  public override void Apply(ControllerModel controller)
  {
    bool hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);

    if (controller.ControllerType.IsSubclassOf(BaseClass))
    {
      bool isAppAware = RuntimeIsAppAware && controller.ControllerType.GetCustomAttribute<ZeroSystemApiAttribute>() == null;

      controller.Selectors[0].AttributeRouteModel = isAppAware ? AppAwareRouteModel : AppUnawareRouteModel;
    }
  }
}