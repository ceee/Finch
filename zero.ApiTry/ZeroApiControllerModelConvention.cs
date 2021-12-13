using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text;
using zero.Extensions;

namespace zero.ApiTry;

public class ZeroApiControllerModelConvention : IControllerModelConvention
{
  readonly AttributeRouteModel AppAwareRouteModel;

  readonly AttributeRouteModel AppUnawareRouteModel;

  readonly Type BaseClass = typeof(AppApiController);

  readonly Type SystemApiType = typeof(ZeroSystemApiAttribute);

  readonly ApiParameterTransformer Transformer = new();


  public ZeroApiControllerModelConvention(string apiPath = "/zero/api")
  {
    AppAwareRouteModel = BuildRouteModel(apiPath, true);
    AppUnawareRouteModel = BuildRouteModel(apiPath, false);
  }


  /// <summary>
  /// Configure routing model for all backoffice controllers
  /// </summary>
  public virtual void Apply(ControllerModel controller)
  {
    bool hasAttributeRouteModels = controller.Selectors.Any(selector => selector.AttributeRouteModel != null);

    if (controller.ControllerType.IsSubclassOf(BaseClass))
    {
      bool isSystemApi = controller.Attributes.Any(x => x.GetType() == SystemApiType);
      controller.Selectors[0].AttributeRouteModel = isSystemApi ? AppUnawareRouteModel : AppAwareRouteModel;
      controller.Filters.Add(new DisableBrowserCacheFilterAttribute());

      foreach (var action in controller.Actions)
      {
        action.RouteParameterTransformer = Transformer;
      }
    }
  }


  protected AttributeRouteModel BuildRouteModel(string apiPath, bool appAware = false)
  {
    StringBuilder path = new();
    path.Append(apiPath.EnsureSurroundedWith('/'));

    if (appAware)
    {
      path.Append("{zero_app_slug}/");
    }

    path.Append("[controller]");

    return new AttributeRouteModel(new RouteAttribute(path.ToString()));
  }
}