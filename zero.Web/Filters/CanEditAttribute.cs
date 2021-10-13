using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Web.Models;

namespace zero.Web.Filters
{
  public class CanEditAttribute : TypeFilterAttribute
  {
    public CanEditAttribute() : base(typeof(CanEditAttributeImpl)) { }


    private class CanEditAttributeImpl : IAsyncResultFilter
    {
      //IToken token;


      public CanEditAttributeImpl()//IToken token)
      {
        //this.token = token;
      }

      public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
      {
        if (context.Result is JsonResult)
        {
          JsonResult result = context.Result as JsonResult;

          if (result.Value is ObsoleteEditModel)
          {
            ObsoleteEditModel model = result.Value as ObsoleteEditModel;

            model.CanEdit = true; // TODO query authorize attrs to get permissions
          }
        }

        await next();
      }
    }
  }
}
