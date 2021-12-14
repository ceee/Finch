using Microsoft.AspNetCore.Builder;

namespace zero.Api.Filters;

public class ApiUnhandledExceptionMiddlewareFilter
{
  public void Configure(IApplicationBuilder applicationBuilder)
  {
    applicationBuilder.UseMiddleware<ApiUnhandledExceptionMiddleware>();
  }
}