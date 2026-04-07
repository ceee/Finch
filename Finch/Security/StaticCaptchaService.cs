using Microsoft.Extensions.DependencyInjection;
using PowCapServer.Abstractions;

namespace Finch.Security;

internal static class StaticCaptchaService
{
  private static IServiceProvider Services { get; set; }

  public static void Configure(IServiceProvider services)
  {
    Services = services;
  }

  public static ICaptchaService Get() => Services.GetRequiredService<ICaptchaService>();
}