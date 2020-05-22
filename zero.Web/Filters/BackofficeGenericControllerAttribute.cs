using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using zero.Core.Extensions;

namespace zero.Web.Filters
{
  public class BackofficeGenericControllerAttribute : Attribute, IControllerModelConvention
  {
    public void Apply(ControllerModel controller)
    {
      if (controller.ControllerName.Contains('`'))
      {
        controller.ControllerName = controller.ControllerName.Split('`')[0].TrimEnd("Controller");   
      }
    }
  }
}
