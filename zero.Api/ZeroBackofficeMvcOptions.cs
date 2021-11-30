using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using zero.Api.Filters;

namespace zero.Api;

internal class ZeroBackofficeMvcOptions : IConfigureOptions<MvcOptions>
{
  IZeroOptions Options { get; set; }

  public ZeroBackofficeMvcOptions(IZeroOptions options)
  {
    Options = options;
  }

  public void Configure(MvcOptions options)
  {
    options.Conventions.Add(new ZeroBackofficeControllerModelConvention(Options.ZeroPath));
    //options.Conventions.Add(new RouteTokenTransformerConvention(new ApiParameterTransformer()));
  }
}