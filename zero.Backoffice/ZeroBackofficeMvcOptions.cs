using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace zero.Backoffice;

class ZeroBackofficeMvcOptions : IConfigureOptions<MvcOptions>
{
  IZeroOptions Options { get; set; }

  public ZeroBackofficeMvcOptions(IZeroOptions options)
  {
    Options = options;
  }

  public void Configure(MvcOptions options)
  {
    string backofficePath = Options.For<BackofficeOptions>().Path;
    options.Conventions.Add(new ZeroBackofficeControllerModelConvention(backofficePath));
  }
}