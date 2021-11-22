namespace zero;

internal class Registrations
{
  public static List<ZeroModule> Modules { get; } = new()
  {
    new ArchitectureModule(),
    new ApplicationsModule(),
    new ContextModule(),
    new CommunicationModule(),
    new ConfigurationModule(),
    new IdentityModule(),
    new LocalizationModule(),
    new MailsModule(),
    new PagesModule(),
    new PersistenceModule(),
    new RenderingModule(),
    new RoutingModule()
  };


  public static List<ZeroPlugin> Plugins { get; } = new()
  {

  };
}