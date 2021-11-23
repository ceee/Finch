using System.Threading.Tasks;

namespace zero.Core.Collections
{
  public class TranslationsCollection : EntityStore<Translation>, ITranslationsCollection
  {
    public TranslationsCollection(IStoreContext<Translation> context) : base(context) { }


    /// <inheritdoc />
    public async Task<string> LoadString(string id)
    {
      return (await Load(id))?.Value;
    }
  }


  public interface ITranslationsCollection : IEntityStore<Translation>
  {
    /// <summary>
    /// Get a translated string by id
    /// </summary>
    Task<string> LoadString(string id);
  }
}
