using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace zero.Core.Auth
{
  public class ZeroAuthorizeAttribute : TypeFilterAttribute
  {

    public ZeroAuthorizeAttribute() : this(String.Empty, true) { }

    public ZeroAuthorizeAttribute(bool enabled) : this(String.Empty, enabled) { }

    public ZeroAuthorizeAttribute(string name, bool enabled) : base(typeof(ZeroAuthorizeFilter))
    {
      Arguments = new object[] { name, enabled };
    }
  }


  public class ZeroAuthorizeFilter : IAuthorizationFilter
  {
    string Name { get; set; }

    bool Enabled { get; set; }


    public ZeroAuthorizeFilter(string name, bool enabled) : base()
    {
      Name = name;
      Enabled = enabled;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
      Console.WriteLine("zeroauthorize: " + (Name ?? "[empty]"));

      // allow anonymous skips all authorization
      if (context.Filters.Any(item => item is IAllowAnonymousFilter) || !Enabled)
      {
        return;
      }

      var httpContext = context.HttpContext;
      var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();

      ClaimsPrincipal user = context.HttpContext.User;

      bool isAuthenticated = user.Identity.IsAuthenticated;
      bool isZeroUser = user.HasClaim(Constants.Auth.Claims.IsZero, Constants.Auth.Claims.IsZero);

      if (!isAuthenticated || !isZeroUser)
      {
        context.Result = new StatusCodeResult(401);
      }
    }
  }
}
