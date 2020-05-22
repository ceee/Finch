using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using zero.Core.Entities;
using zero.Web.Controllers;

namespace zero.Web
{
  public class ApiControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
  {
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
      var ctrls = feature.Controllers;

      var type = typeof(ApplicationsController<>).MakeGenericType(typeof(Application)).GetTypeInfo();

      feature.Controllers.Add(type);
      //var controllerType = typeof(GenericController<>).MakeGenericType(entityType.AsType()).GetTypeInfo();

      //feature.Controllers.Add()
      //var typeName = entityType.Name + "Controller";

      //// Check to see if there is a "real" controller for this class
      //if (!feature.Controllers.Any(t => t.Name == typeName))
      //{
      //  // Create a generic controller for this type
      //  feature.Controllers.Add(controllerType);
      //}
    }
  }
}
