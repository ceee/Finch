using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text;
using zero.Api.Controllers;

namespace zero.Backoffice;

public class ZeroBackofficeControllerModelConvention : ZeroApiControllerModelConvention
{
  public ZeroBackofficeControllerModelConvention(string path, bool isAppAware = false) : base(path, isAppAware)
  {
    BaseClassType = typeof(ZeroBackofficeController);
  }


  protected override AttributeRouteModel BuildRouteModel(string pathSegment, bool appAware = false)
  {
    StringBuilder path = new();
    path.Append(pathSegment.EnsureSurroundedWith('/'));

    path.Append("{zero_app_key}/");
    // TODO add route constraint which only allows registered app-ids
    // see https://nemi-chand.github.io/creating-custom-routing-constraint-in-aspnet-core-mvc/

    path.Append("backoffice/[controller]");

    return new AttributeRouteModel(new RouteAttribute(path.ToString()));
  }
}