using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace zero.Backoffice.Controllers;

public class ZeroBackofficeControllerModelConvention : IApplicationModelConvention
{
  readonly AttributeRouteModel RouteModel;

  readonly Type BaseClass = typeof(ZeroBackofficeController);


  public ZeroBackofficeControllerModelConvention(string backofficePath)
  {
    RouteModel = new AttributeRouteModel(new RouteAttribute(backofficePath.TrimEnd('/') + "/api/[controller]"));
  }


  /// <summary>
  /// Configure routing model for all backoffice controllers
  /// </summary>
  public void Apply(ApplicationModel application)
  {
    foreach (var controller in application.Controllers)
    {
      bool hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);

      if (controller.ControllerType.IsSubclassOf(BaseClass))
      {
        controller.Selectors[0].AttributeRouteModel = RouteModel;
      }
    }
  }
}