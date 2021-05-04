//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Threading.Tasks;
//using zero.Core.Api;
//using zero.Core.Entities;
//using zero.Web.Models;

//namespace zero.Web.Filters
//{
//  public class ZeroBackofficeAttribute : TypeFilterAttribute
//  {
//    public bool IsTyped { get; set; }


//    public ZeroBackofficeAttribute(bool isTyped = true) : base(typeof(ZeroBackofficeAttributeImpl))
//    {
//      IsTyped = isTyped;
//    }



//    private class ZeroBackofficeAttributeImpl : IAsyncResultFilter
//    {
//      IToken token;


//      public ZeroBackofficeAttributeImpl(IToken token)
//      {
//        this.token = token;
//      }

//      public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
//      {
//        JsonResult result = context.Result as JsonResult;

//        if (result != null)
//        {
//          ZeroEntity model = result.Value as ZeroEntity;

//          if (model != null)
//          {
//            Type type = result.Value.GetType();

//            EditModel editModel = new EditModel()
//            {
//              Entity = result.Value,
//              Token = await token.Get(model),
//              IsAppAware = typeof(IAppAwareEntity).IsAssignableFrom(type),
//              CanBeShared = typeof(IAppAwareShareableEntity).IsAssignableFrom(type),
//              CanDelete = false, // TODO 
//            };
//          }
//        }

//        await next();
//      }
//    }
//  }
//}
