using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace zero.Api.Filters;

public class ApiResponseFilterAttribute : ResultFilterAttribute
{
  public override void OnResultExecuting(ResultExecutingContext context)
  {
    if (context.Result is ObjectResult result)
    {
      if (result.Value is Paged paged)
      {
        result.Value = GetPagedResult(context, paged);
      }

      if (result.Value is Result model)
      {

      }

      if (result.Value is ApiResponse apiResponse)
      {
        apiResponse.Success = true;
        apiResponse.Status = result.StatusCode.Value;
        apiResponse.Metadata =  GetMetadata(context);
      }
    }
  }


  PagedDataApiResponse GetPagedResult(ResultExecutingContext context, Paged paged)
  {
    return new PagedDataApiResponse()
    {
      Page = paged.Page,
      PageSize = paged.PageSize,
      TotalPages = paged.TotalPages,
      TotalItems = paged.TotalItems
    };
  }


  //PagedDataApiResponse GetModelResult(ResultExecutingContext context, Result model)
  //{
  //  return new DataApiResponse()
  //  {
      
  //  };
  //}


  ApiResponseMetadata GetMetadata(ResultExecutingContext context)
  {
    DateTimeOffset started = (DateTimeOffset)context.HttpContext.Items["zero.action.started"];
    TimeSpan duration = DateTimeOffset.Now - started;

    return new()
    {
      Duration = duration,
      RequestDate = started
    };
  }


    //if (context.Result is ObjectResult result && result.StatusCode.HasValue)
    //{
    //  if (typeof(ZeroIdEntity).IsAssignableFrom(result.DeclaredType))
    //  {
    //    result.Value = new ModelApiResponse()
    //    {
    //      Model = result.Value,
    //      Success = true,
    //      Status = result.StatusCode.Value,
    //      ChangeToken = context.HttpContext.Items[ApiConstants.ChangeToken] as string,
    //      Metadata = new ModelApiResponseMetadata()
    //      {
    //        RequestDate = started,
    //        Duration = duration,
    //        Token = IdGenerator.Create(32)
    //      }
    //    };
    //  }
    //  else if (typeof(Paged).IsAssignableFrom(result.DeclaredType))
    //  {
    //    Paged paged = result.Value as Paged;
    //    result.Value = new PagedApiResponse()
    //    {
    //      Items = paged.GetItems(),
    //      Paging = new()
    //      {
    //        Page = paged.Page,
    //        PageSize = paged.PageSize,
    //        TotalItems = paged.TotalItems,
    //        TotalPages = paged.TotalPages,
    //        HasMore = paged.HasMore
    //      },
    //      Success = true,
    //      Status = result.StatusCode.Value,
    //      Metadata = new ModelApiResponseMetadata()
    //      {
    //        RequestDate = started,
    //        Duration = duration,
    //        Token = IdGenerator.Create(32)
    //      }
    //    };
    //  }
    //}
}