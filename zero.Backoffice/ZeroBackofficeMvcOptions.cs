using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace zero.Backoffice;

internal class ZeroBackofficeMvcOptions : IConfigureOptions<MvcOptions>
{
  IZeroOptions Options { get; set; }

  public ZeroBackofficeMvcOptions(IZeroOptions options)
  {
    Options = options;
  }

  public void Configure(MvcOptions options)
  {
    options.Conventions.Add(new ZeroBackofficeControllerModelConvention(Options.ZeroPath + "/api", Options.Applications.Count > 1));
  }
}