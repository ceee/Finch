using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;

namespace zero.Web.Controllers
{
  public class ZeroBackofficeControllerModelConvention : IControllerModelConvention
  {
    readonly AttributeRouteModel RouteModel;

    readonly Type BaseClass = typeof(BackofficeController);


    public ZeroBackofficeControllerModelConvention(string backofficePath)
    {
      RouteModel = new AttributeRouteModel(new RouteAttribute(backofficePath + "/api/[controller]/[action]"));
    }


    public void Apply(ControllerModel controller)
    {
      if (!controller.ControllerType.IsSubclassOf(BaseClass))
      {
        return;
      }

      foreach (var selector in controller.Selectors)
      {
        selector.AttributeRouteModel = RouteModel;
      }
    }
  }
}
