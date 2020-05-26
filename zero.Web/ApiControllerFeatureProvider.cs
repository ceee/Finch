using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using zero.Core;
using zero.Web.Filters;

namespace zero.Web
{
  public class ApiControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
  {
    public ApiControllerFeatureProvider()
    {

    }


    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

      IEnumerable<Type> candidates = assemblies
        .Where(assembly => !assembly.IsDynamic)
        .SelectMany(assembly => assembly.GetExportedTypes().Where(x => x.GetCustomAttributes<BackofficeGenericControllerAttribute>().Any() && x.ContainsGenericParameters));

      foreach (Type candidate in candidates)
      {
        Type genericType = candidate.GetGenericTypeDefinition();
        Type[] arguments = genericType.GetGenericArguments();
        Type desiredInterface = arguments.FirstOrDefault()?.GetInterfaces().FirstOrDefault();

        if (desiredInterface == null)
        {
          continue;
        }

        Type implementation = EntityMap.Get(desiredInterface);
        TypeInfo type = candidate.MakeGenericType(implementation).GetTypeInfo();
        
        feature.Controllers.Add(type);
      }
    }
  }
}
