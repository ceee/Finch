using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
  }
}