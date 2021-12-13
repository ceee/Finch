using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;
using System.Text;

namespace zero.Api.Controllers;

public class ZeroApiControllerModelConvention : IControllerModelConvention
{
  protected Type BaseClassType { get; set; } = typeof(ZeroApiController);

  readonly AttributeRouteModel AppAwareRouteModel;

  readonly AttributeRouteModel AppUnawareRouteModel;

  readonly Type SystemApiType = typeof(ZeroSystemApiAttribute);

  readonly ApiParameterTransformer Transformer = new();

  readonly bool RuntimeIsAppAware = false;


  public ZeroApiControllerModelConvention(string path, bool isAppAware = false)
  {
    RuntimeIsAppAware = isAppAware;
    AppAwareRouteModel = BuildRouteModel(path, true);
    AppUnawareRouteModel = BuildRouteModel(path, false);
  }


  /// <summary>
  /// Configure routing model for all backoffice controllers
  /// </summary>
  public virtual void Apply(ControllerModel controller)
  {
    bool hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);

    if (controller.ControllerType.IsSubclassOf(BaseClassType))
    {
      bool isAppAware = RuntimeIsAppAware && controller.ControllerType.GetCustomAttribute(SystemApiType) == null;

      controller.Selectors[0].AttributeRouteModel = isAppAware ? AppAwareRouteModel : AppUnawareRouteModel;
      controller.Filters.Add(new DisableBrowserCacheFilterAttribute());

      foreach (var action in controller.Actions)
      {
        action.RouteParameterTransformer = Transformer;
      }
    }
  }


  protected AttributeRouteModel BuildRouteModel(string pathSegment, bool appAware = false)
  {
    StringBuilder path = new();
    path.Append(pathSegment.EnsureSurroundedWith('/'));

    if (appAware)
    {
      path.Append("{zero_app_key}/");
      // TODO add route constraint which only allows registered app-ids
      // see https://nemi-chand.github.io/creating-custom-routing-constraint-in-aspnet-core-mvc/
    }

    path.Append("[controller]");

    return new AttributeRouteModel(new RouteAttribute(path.ToString()));
  }
}