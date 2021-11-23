using System.Threading.Tasks;

namespace zero.Core.Collections
{
  public class TranslationsCollection : EntityCollection<Translation>, ITranslationsCollection
  {
    public TranslationsCollection(ICollectionContext<Translation> context) : base(context) { }


    /// <inheritdoc />
    public async Task<string> LoadString(string id)
    {
      return (await Load(id))?.Value;
    }
  }


  public interface ITranslationsCollection : IEntityCollection<Translation>
  {
    /// <summary>
    /// Get a translated string by id
    /// </summary>
    Task<string> LoadString(string id);
  }
}
