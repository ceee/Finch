namespace zero.Api;

public static class ZeroBuilderExtensions
{
  public static ZeroBuilder AddApi(this ZeroBuilder builder)
  {
    return builder.AddPlugin<ZeroApiPlugin>();
  }
}