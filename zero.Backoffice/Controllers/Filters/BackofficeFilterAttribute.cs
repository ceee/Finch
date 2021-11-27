using Microsoft.AspNetCore.Mvc.Filters;

namespace zero.Backoffice.Controllers;

public class BackofficeFilterAttribute : IActionFilter
{
  const string SCOPE_KEY = "scope";


  public void OnActionExecuting(ActionExecutingContext context)
  {
    Type type = context.Controller.GetType();

    if (typeof(ZeroBackofficeController).IsAssignableFrom(type))
    {
      if (context.HttpContext.Request.Query.TryGetValue(SCOPE_KEY, out var scope))
      {
        //(context.Controller as ZeroBackofficeController).OnScopeChanged(scope.ToString()); 
      }
    }
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
  }
}