using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace zero.Api.Filters;

public class ApiResponseFilterAttribute : ResultFilterAttribute
{
  public override void OnResultExecuting(ResultExecutingContext context)
  {
    DateTimeOffset started = (DateTimeOffset)context.HttpContext.Items["zero.action.started"];
    TimeSpan duration = DateTimeOffset.Now - started;

    if (context.Result is ObjectResult result && result.StatusCode.HasValue)
    {
      if (typeof(ZeroIdEntity).IsAssignableFrom(result.DeclaredType))
      {
        result.Value = new ModelApiResponse()
        {
          Model = result.Value,
          Success = true,
          Status = result.StatusCode.Value,
          ChangeToken = context.HttpContext.Items[ApiConstants.ChangeVector] as string,
          Metadata = new ModelApiResponseMetadata()
          {
            RequestDate = started,
            Duration = duration,
            Token = IdGenerator.Create(32)
          }
        };
      }
      else if (typeof(Paged).IsAssignableFrom(result.DeclaredType))
      {
        Paged paged = result.Value as Paged;
        result.Value = new PagedApiResponse()
        {
          Items = paged.GetItems(),
          Paging = new()
          {
            Page = paged.Page,
            PageSize = paged.PageSize,
            TotalItems = paged.TotalItems,
            TotalPages = paged.TotalPages,
            HasMore = paged.HasMore
          },
          Success = true,
          Status = result.StatusCode.Value,
          Metadata = new ModelApiResponseMetadata()
          {
            RequestDate = started,
            Duration = duration,
            Token = IdGenerator.Create(32)
          }
        };
      }
    }
  }
}