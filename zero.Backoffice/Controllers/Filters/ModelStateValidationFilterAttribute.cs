using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace zero.Backoffice.Controllers;

public class ModelStateValidationFilterAttribute : IActionFilter
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid)
    {
      context.Result = new BadRequestObjectResult(context.ModelState);
    }
  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
  }
}