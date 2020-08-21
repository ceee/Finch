using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using zero.Core;
using zero.Core.Assemblies;
using zero.Core.Entities;
using zero.Web.Controllers;
using zero.Web.Filters;

namespace zero.Web
{
  public class ApiControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
  {
    IServiceCollection ServiceCollection;

    public ApiControllerFeatureProvider(IServiceCollection serviceCollection)
    {
      ServiceCollection = serviceCollection;
    }


    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
      var pagetype = typeof(IPage);
      var page = ServiceCollection.FirstOrDefault(x => x.ServiceType.Equals(pagetype));
      IEnumerable<TypeInfo> candidates = AssemblyDiscovery.Current.GetTypes<ISupportsGenericsController>(parts, true).Where(x => x.ContainsGenericParameters);

      foreach (TypeInfo candidate in candidates)
      {
        Type genericType = candidate.GetGenericTypeDefinition();
        Type[] genericArguments = genericType.GetGenericArguments();

        List<Type> concreteArguments = new List<Type>();

        bool isValid = true;

        foreach (Type arg in genericArguments)
        {
          Type requiredService = arg.GetInterfaces().FirstOrDefault();

          if (requiredService == null)
          {
            isValid = false;
            break;
          }

          TypeInfo concreteArgument = null;

          ServiceDescriptor descriptor = ServiceCollection.FirstOrDefault(x => x.ServiceType.Equals(requiredService));

          if (descriptor != null)
          {
            concreteArgument = descriptor.ImplementationType?.GetTypeInfo();
          }

          if (concreteArgument == null)
          {
            IEnumerable<TypeInfo> concreteArgumentCandidates = AssemblyDiscovery.Current.GetTypes(requiredService, parts);
            concreteArgument = concreteArgumentCandidates.FirstOrDefault();

            if (concreteArgument == null)
            {
              isValid = false;
              break;
            }
          }

          concreteArguments.Add(concreteArgument);
        }

        if (!isValid)
        {
          continue;
        }

        TypeInfo type = candidate.MakeGenericType(concreteArguments.ToArray()).GetTypeInfo();
        feature.Controllers.Add(type);
      }
    }
  }
}
