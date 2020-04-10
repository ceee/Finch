using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace zero.Core.Auth
{
  public class ZeroAuthorizeAttribute : TypeFilterAttribute
  {
    
    public ZeroAuthorizeAttribute(string name) : base(typeof(ZeroAuthorizeFilter))
    {
      Arguments = new object[] { name };
    }
  }


  public class ZeroAuthorizeFilter : AuthorizeFilter
  {
    string Name { get; set; }


    public ZeroAuthorizeFilter(string name)
    {
      Name = name;
    }
    //public ZeroAuthorizeFilter(IAuthorizationPolicyProvider provider)
    //    : base(provider, new[] { new AuthorizeData(Constants.AzureAdPolicy) }) { }

    public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
      await Task.Delay(0);
      //await base.OnAuthorizationAsync(context);

      Console.WriteLine("zeroauthorize: " + Name);

      context.Result = new AcceptedResult();
      //context.Result = new ForbidResult();
      //await base.OnAuthorizationAsync(context);

      //var username = context.HttpContext.User.Identity.Name;

      //Console.WriteLine($"{username} just logged in!");
      
    }
  }
}
