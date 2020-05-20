//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using Raven.Client.Documents.Session;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using zero.Core.Entities;
//using zero.Core.Identity;

//namespace zero.Core.Extensions
//{
//  public static class HttpContextExtensions
//  {
//    public static string GetCurrentAppId(this HttpContext context)
//    {
//      ClaimsPrincipal user = context.User;

//      var permissions = user.Claims.Where(x => x.Type == Constants.Auth.Claims.Permission && );

//      user.HasClaim(Constants.Auth.Claims.Permission, Permissions.Applications + ":" + PermissionsValue.Write)

//      // this is not the final decision,
//      // as we need to check permissions too
//      string currentAppId = user.CurrentAppId;

//      // set to default app id when nothing selected yet
//      if (currentAppId.IsNullOrEmpty())
//      {
//        currentAppId = user.AppId;
//      }

//      UserManager<User> manager;

//      manager.cla
//    }
//  }
//}
