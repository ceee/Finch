using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace zero.Identity;

public class ZeroAuthorizeAttribute : TypeFilterAttribute
{
  public ZeroAuthorizeAttribute() : this(true, String.Empty) { }

  public ZeroAuthorizeAttribute(bool enabled) : this(enabled, String.Empty) { }

  public ZeroAuthorizeAttribute(params string[] permissionKeys) : this(true, permissionKeys) { }

  public ZeroAuthorizeAttribute(bool enabled, params string[] permissionKeys) : base(typeof(ZeroAuthorizeFilter))
  {
    Arguments = new object[] { enabled, permissionKeys };
  }
}


public class ZeroAuthorizeFilter : IAuthorizationFilter
{
  IEnumerable<string> PermissionKeys { get; set; }

  bool Enabled { get; set; }


  public ZeroAuthorizeFilter(bool enabled, IEnumerable<string> permissionKeys) : base()
  {
    PermissionKeys = permissionKeys;
    Enabled = enabled;
  }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    // allow anonymous skips all authorization
    if (true || !Enabled) // context.Filters.Any(item => item is IAllowAnonymousFilter) || 
    {
      return;
    }

    // get all auth filters
    IEnumerable<ZeroAuthorizeFilter> authFilters = context.Filters.Where(filter => filter is ZeroAuthorizeFilter).Select(filter => filter as ZeroAuthorizeFilter);

    // find parent auth filter in case the permission key is empty
    //if (Permission.IsNullOrEmpty())
    //{
    //  ZeroAuthorizeFilter parentFilter = authFilters.LastOrDefault(filter => filter.Enabled && !filter.Permission.IsNullOrEmpty());

    //  if (parentFilter != null)
    //  {
    //    Permission = parentFilter.Permission;
    //  }
    //  else
    //  {
    //    throw new InvalidOperationException("The ZeroAuthorize attribute requires a permission key if it has no parent where it can inherit the key");
    //  }
    //}

    // find all filters which could possible interrupt/override this filter
    // these are filters which handle the same permission or disable authorization
    ZeroAuthorizeFilter[] siblingFilters = authFilters
      .Where(filter => !filter.Enabled) //|| filter.PermissionKey.Equals(PermissionKey, StringComparison.InvariantCultureIgnoreCase))
      .ToArray();

    // get index of the current filter
    int currentIndex = Array.IndexOf(siblingFilters, this);
      
    // do not run this filter if it is overridden
    if (siblingFilters.Length > 1 && currentIndex < siblingFilters.Length - 1)
    {
      return;
    }

    ClaimsPrincipal user = context.HttpContext.User;

    bool isAuthenticated = user.Identity.IsAuthenticated;
    bool isZeroUser = user.HasClaim(Constants.Auth.Claims.IsZero, PermissionsValue.True);

    if (!isAuthenticated || !isZeroUser)
    {
      context.Result = new StatusCodeResult(401);
      return;
    }

    // check claims
    //if (!PermissionKey.IsNullOrEmpty())
    //{
    //  bool isSuperUser = false; // TODO user.HasClaim(Constants.Auth.Claims.IsSuper, PermissionsValue.True);
    //  bool hasPassed = isSuperUser;

    //  if (!isSuperUser)
    //  {
    //    // automatically request write permission for methods which start with `Save` or `Delete` or `Post` or `Put`
    //    string actionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName;

    //    //if (actionName.StartsWith("Create"))
    //    //{
    //    //  PermissionValues.Add(PermissionsValue.Create);
    //    //}
    //    //if (actionName.StartsWith("Update") || actionName.StartsWith("Save"))
    //    //{
    //    //  PermissionValues.Add(PermissionsValue.Update);
    //    //}
    //    //if (actionName.StartsWith("Delete") || actionName.StartsWith("Remove"))
    //    //{
    //    //  PermissionValues.Add(PermissionsValue.Delete);
    //    //}

    //    //foreach (string value in PermissionValues)
    //    //{
    //    //  bool fulfillsClaim = user.HasClaim(Constants.Auth.Claims.Permission, PermissionKey + ":" + value);

    //    //  if (!fulfillsClaim && value == PermissionsValue.Read)
    //    //  {
    //    //    fulfillsClaim = user.HasClaim(Constants.Auth.Claims.Permission, PermissionKey + ":" + PermissionsValue.Update);
    //    //  }

    //    //  if (fulfillsClaim)
    //    //  {
    //    //    hasPassed = true;
    //    //    break;
    //    //  }
    //    //}
    //  }

      //if (!hasPassed) // TODO 
      //{
      //  context.Result = new StatusCodeResult(403);
      //}
    //}
  }
}
