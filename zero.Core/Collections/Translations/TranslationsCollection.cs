using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public class TranslationsCollection : CollectionBase<Translation>, ITranslationsCollection
  {
    public TranslationsCollection(ICollectionContext context, IValidator<Translation> validator) : base(context, validator) { }


    /// <inheritdoc />
    public async Task<string> GetStringById(string id)
    {
      return (await GetById(id))?.Value;
    }

    /// <inheritdoc />
    public override IAsyncEnumerable<Translation> Stream()
    {
      return base.Stream(q => q.OrderByDescending(x => x.Name));
    }
  }


  public interface ITranslationsCollection : ICollectionBase<Translation>
  {
    /// <summary>
    /// Get a translated string by id
    /// </summary>
    Task<string> GetStringById(string id);
  }
}
