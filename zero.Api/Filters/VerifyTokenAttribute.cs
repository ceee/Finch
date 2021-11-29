//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Threading.Tasks;
//using zero.Core.Api;
//using zero.Web.Models;

//namespace zero.Web.Controllers
//{
//  public class VerifyTokenAttribute : TypeFilterAttribute
//  {
//    public VerifyTokenAttribute(string key = "model") : base(typeof(VerifyTokenAttributeImpl))
//    {
//      Arguments = new object[] { key };
//    }


//    private class VerifyTokenAttributeImpl : IAsyncActionFilter
//    {
//      IToken token;
//      string key;


//      public VerifyTokenAttributeImpl(IToken token, string key)
//      {
//        this.token = token;
//        this.key = key;
//      }


//      public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//      {
//        if (context.ActionArguments.ContainsKey(key))
//        {
//          object argument = context.ActionArguments[key];

//          if (argument is ObsoleteEditModel)
//          {
//            ObsoleteEditModel model = (argument as ObsoleteEditModel);
//            string tokenId = model.Meta?.Token;

//            bool isVerified = await token.Verify(model.Id, tokenId);

//            if (!isVerified)
//            {
//              context.Result = new StatusCodeResult(409);
//              return;
//            }
//          }
//        }

//        await next();
//      }
//    }
//  }
//}
