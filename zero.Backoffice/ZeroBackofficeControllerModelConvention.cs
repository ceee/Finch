using zero.Api.Controllers;

namespace zero.Backoffice;

public class ZeroBackofficeControllerModelConvention : ZeroApiControllerModelConvention
{
  public ZeroBackofficeControllerModelConvention(string path, bool isAppAware = false) : base(path, isAppAware)
  {
    BaseClassType = typeof(ZeroBackofficeController);
  }
}