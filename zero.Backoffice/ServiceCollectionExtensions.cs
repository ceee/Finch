using Microsoft.Extensions.DependencyInjection;

namespace zero.Backoffice;

public static class ServiceCollectionExtensions
{
  public static ZeroBuilder AddBackoffice(this ZeroBuilder builder)
  {
    return builder.AddPlugin<ZeroBackofficePlugin>();
  }
}