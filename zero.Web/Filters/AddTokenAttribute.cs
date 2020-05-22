using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Web.Models;

namespace zero.Web.Filters
{
  public class AddTokenAttribute : TypeFilterAttribute
  {
    public AddTokenAttribute() : base(typeof(AddTokenAttributeImpl)) { }


    private class AddTokenAttributeImpl : IAsyncResultFilter
    {
      IToken token;


      public AddTokenAttributeImpl(IToken token)
      {
        this.token = token;
      }

      public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
      {
        if (context.Result is JsonResult)
        {
          JsonResult result = context.Result as JsonResult;

          if (result.Value is ObsoleteEditModel)
          {
            ObsoleteEditModel model = result.Value as ObsoleteEditModel;

            model.Meta = model.Meta ?? new EditModelMeta();
            model.Meta.Token = await token.Get(model.Id);
          }

          if (result.Value is ObsoleteEditModel)
          {
            ObsoleteEditModel model = result.Value as ObsoleteEditModel;

            model.Meta = model.Meta ?? new EditModelMeta();
            model.Meta.Token = await token.Get(model.Id);
          }
        }

        await next();
      }
    }
  }
}
