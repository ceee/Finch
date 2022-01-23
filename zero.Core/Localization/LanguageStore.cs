namespace zero.Localization;

public class LanguageStore : EntityStore<Language>, ILanguageStore
{
  public LanguageStore(IStoreContext context) : base(context) { }
}


public interface ILanguageStore : IEntityStore<Language> { }