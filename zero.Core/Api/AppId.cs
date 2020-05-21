//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using zero.Core.Entities;

//namespace zero.Core.Api
//{
//  public class AppIdService
//  {
//    protected UserManager<User> UserManager { get; private set; }

//    protected IHttpContextAccessor HttpContextAccessor { get; set; }

//    protected ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


//    public AppIdService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
//    {
//      UserManager = userManager;
//      HttpContextAccessor = httpContextAccessor;
//    }


//    /// <inheritdoc />
//    public string[] GetAllowedAppIds()
//    {
//      return Principal.Claims
//        .Where(claim => claim.Type == Constants.Auth.Claims.Permission && (prefix == null || claim.Value.StartsWith(prefix)))
//        .Select(claim => Permission.FromClaim(claim, prefix))
//        .ToList();

//      //return GetPermissions(Permissions.Applications).Where(x => x.CanRead).Select(x => x.NormalizedKey).ToArray();
//    }
//  }
//}
