namespace zero.Backoffice;

internal class Registrations
{
  public static List<ZeroModule> Modules { get; } = new()
  {
    new CountriesModule(),
    new SearchModule(),
    new BackofficeUICompositionModule(),
    new BackofficeDevServerModule()
  };
}