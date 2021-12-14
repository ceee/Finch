using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace zero.Api.Filters;

public class ApiResponseFilterAttribute : ResultFilterAttribute
{
  public override void OnResultExecuting(ResultExecutingContext context)
  {
    if (context.Result is ObjectResult result)
    {
      // format paged results
      if (result.Value is Paged paged)
      {
        result.Value = new PagedDataApiResponse()
        {
          Success = true,
          Status = result.StatusCode.Value,
          Page = paged.Page,
          PageSize = paged.PageSize,
          TotalPages = paged.TotalPages,
          TotalItems = paged.TotalItems,
          Data = paged.GetItems()
        };
      }

      // format patch results
      else if (result.Value is Result model)
      {
        ApiResponse response = new DataApiResponse();

        if (!model.IsSuccess)
        {
          response = new ErrorApiResponse()
          {
            Errors = model.Errors.Select(x => new ErrorApiResponseError()
            {
              Category = ApiErrorCodes.Categories.Validation,
              Code = "// TODO",
              Property = x.Property,
              Message = x.Message
            }).ToList()
          };
        }
        else if (context.HttpContext.Items.TryGetValue(ApiConstants.ChangeToken, out object tokenObj) && tokenObj is string token)
        {
          response = new TokenizedDataApiResponse()
          {
            Data = model.GetModel(),
            ChangeToken = token
          };
        }
        else
        {
          response = new DataApiResponse()
          {
            Data = model.GetModel()
          };
        }

        response.Success = model.IsSuccess;
        response.Status = !model.IsSuccess ? StatusCodes.Status400BadRequest : result.StatusCode.Value;

        result.StatusCode = response.Status;
        result.Value = response;
      }

      // format model results
      else
      {
        DataApiResponse response = new();

        if (context.HttpContext.Items.TryGetValue(ApiConstants.ChangeToken, out object tokenObj) && tokenObj is string token)
        {
          response = new TokenizedDataApiResponse() { ChangeToken = token };
        }

        response.Success = true;
        response.Status = result.StatusCode.Value;
        response.Data = result.Value;
        result.Value = response;
      }

      // append metadata
      if (result.Value is ApiResponse apiResponse)
      {
        apiResponse.Metadata =  GetMetadata(context);
        context.HttpContext.Response.Headers["X-Variant"] = "api-response";
      }
    }
  }


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