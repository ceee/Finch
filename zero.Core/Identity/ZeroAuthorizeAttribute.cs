using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public class ZeroAuthorizeAttribute : TypeFilterAttribute
  {
    public ZeroAuthorizeAttribute() : this(true, String.Empty, String.Empty) { }

    public ZeroAuthorizeAttribute(bool enabled) : this(enabled, String.Empty, String.Empty) { }

    public ZeroAuthorizeAttribute(string permission, string value) : this(true, permission, value) { }

    public ZeroAuthorizeAttribute(bool enabled, string permission, params string[] values) : base(typeof(ZeroAuthorizeFilter))
    {
      Arguments = new object[] { enabled, permission, values };
    }
  }


  public class ZeroAuthorizeFilter : IAuthorizationFilter
  {
    string Permission { get; set; }

    string[] PermissionValues { get; set; }

    bool Enabled { get; set; }


    public ZeroAuthorizeFilter(bool enabled, string permission, string[] values) : base()
    {
      Permission = permission;
      PermissionValues = values;
      Enabled = enabled;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // allow anonymous skips all authorization
      if (context.Filters.Any(item => item is IAllowAnonymousFilter) || !Enabled)
      {
        return;
      }

      var httpContext = context.HttpContext;
      var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();

      ClaimsPrincipal user = context.HttpContext.User;

      bool isAuthenticated = user.Identity.IsAuthenticated;
      bool isZeroUser = user.HasClaim(Constants.Auth.Claims.IsZero, PermissionsValue.True);

      if (!isAuthenticated || !isZeroUser)
      {
        context.Result = new StatusCodeResult(401);
        return;
      }

      // check claims
      if (!Permission.IsNullOrEmpty())
      {
        bool isSuperUser = false; // TODO user.HasClaim(Constants.Auth.Claims.IsSuper, PermissionsValue.True);
        bool hasPassed = isSuperUser;

        if (!isSuperUser)
        {
          foreach (string value in PermissionValues)
          {
            bool fulfillsClaim = user.HasClaim(Constants.Auth.Claims.Permission, Permission + ":" + value);

            if (!fulfillsClaim && value == PermissionsValue.Read)
            {
              fulfillsClaim = user.HasClaim(Constants.Auth.Claims.Permission, Permission + ":" + PermissionsValue.Write);
            }

            if (fulfillsClaim)
            {
              hasPassed = true;
              break;
            }
          }
        }

        if (!hasPassed)
        {
          context.Result = new StatusCodeResult(403);
        }
      }
    }
  }
}
