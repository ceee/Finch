using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using zero.Core.Extensions;

namespace zero.Web.Filters
{
  public class BackofficeGenericControllerAttribute : Attribute, IControllerModelConvention
  {
    const char BACKTICK = '`';

    const string CONTROLLER = "Controller";


    public void Apply(ControllerModel controller)
    {
      if (controller.ControllerName.Contains(BACKTICK))
      {
        controller.ControllerName = controller.ControllerName.Split(BACKTICK)[0].TrimEnd(CONTROLLER);   
      }
    }
  }
}
