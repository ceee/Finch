using Microsoft.AspNetCore.Mvc;

namespace zero.Backoffice.Controllers;

[ZeroAuthorize]
[DisableBrowserCache]
public abstract class ZeroBackofficeController : ControllerBase
{
  /// <summary>
  /// Create an edit model with associated metadata and permissions from a model
  /// </summary>
  protected DisplayModel<T> GetEditModel<T>(T model)
  {
    return GetEditModel<T, DisplayModel<T>>(new DisplayModel<T>() { Entity = model });
  }


  /// <summary>
  /// Create an edit model with associated metadata and permissions from a model
  /// </summary>
  protected TWrapper GetEditModel<T, TWrapper>(TWrapper editModel) where TWrapper : DisplayModel<T>, new()
  {
    editModel.Meta = new()
    {
      Token = null,
      IsShared = false
    };

    editModel.Permissions = new()
    {
      CanCreate = true,
      CanEdit = true,
      CanDelete = true
    };

    return editModel;
  }
}
