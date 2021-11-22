namespace zero.Architecture;

internal class ZeroModuleInitializer
{
  public static void RegisterAll(IZeroModuleConfiguration config)
  {
    foreach (ZeroModule module in Registrations.Modules)
    {
      module.Register(config);
    }
  }

  public static void ConfigureAll(IZeroOptions options)
  {
    foreach (ZeroModule module in Registrations.Modules)
    {
      module.Configure(options);
    }
  }
}