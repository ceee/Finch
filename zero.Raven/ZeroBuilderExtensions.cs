using zero.Raven;

namespace zero;

public static class ZeroBuilderExtensions
{
  public static ZeroBuilder AddRavenDb(this ZeroBuilder builder)
  {
    builder.AddModule<ZeroRavenModule>();
    return builder;
  }
}