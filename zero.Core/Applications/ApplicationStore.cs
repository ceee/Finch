namespace zero.Applications;

public class ApplicationStore : EntityStore<Application>, IApplicationStore
{
  public ApplicationStore(IStoreContext context) : base(context)
  {
    Config.Database = Options.For<RavenOptions>().Database;
  }
}

public interface IApplicationStore : IEntityStore<Application>
{
}