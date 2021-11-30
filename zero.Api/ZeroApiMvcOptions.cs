using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace zero.Api;

internal class ZeroApiMvcOptions : IConfigureOptions<MvcOptions>
{
  IZeroOptions Options { get; set; }

  public ZeroApiMvcOptions(IZeroOptions options)
  {
    Options = options;
  }

  public void Configure(MvcOptions options)
  {
    options.Conventions.Add(new ZeroApiControllerModelConvention(Options.ZeroPath, isAppAware: Options.For<ApplicationOptions>().EnableMultiple));
    options.Conventions.Add(new RouteTokenTransformerConvention(new ApiParameterTransformer()));
  }
}