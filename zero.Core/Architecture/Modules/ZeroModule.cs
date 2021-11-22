namespace zero.Architecture;

public class ZeroModule
{
  public virtual void Register(IZeroModuleConfiguration config) { }

  public virtual void Configure(IZeroOptions options) { }
}