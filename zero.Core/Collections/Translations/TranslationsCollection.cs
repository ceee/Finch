using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public class TranslationsCollection : CollectionBase<ITranslation>, ITranslationsCollection
  {
    public TranslationsCollection(IZeroContext context, ICollectionInterceptorHandler interceptor, IValidator<ITranslation> validator) : base(context, interceptor, validator) { }


    /// <inheritdoc />
    public async Task<string> GetStringById(string id)
    {
      return (await GetById(id))?.Value;
    }

    /// <inheritdoc />
    public override IAsyncEnumerable<ITranslation> Stream()
    {
      return base.Stream(q => q.OrderByDescending(x => x.Name));
    }
  }


  public interface ITranslationsCollection : ICollectionBase<ITranslation>
  {
    /// <summary>
    /// Get a translated string by id
    /// </summary>
    Task<string> GetStringById(string id);
  }
}
