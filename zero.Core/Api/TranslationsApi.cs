using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class TranslationsApi : AppAwareBackofficeApi, ITranslationsApi
  {
    public TranslationsApi(IBackofficeStore store) : base(store)
    {
      AllowShared = true;
    }


    /// <inheritdoc />
    public async Task<Translation> GetById(string id)
    {
      return await GetById<Translation>(id);
    }


    /// <inheritdoc />
    public async Task<IList<Translation>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Translation>()
          .OrderByDescending(x => x.CreatedDate)
          .Scope(Scope)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<Translation>> GetByQuery(ListQuery<Translation> query)
    {
      query.SearchFor(entity => entity.Key, entity => entity.Value);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Translation>().Scope(Scope).ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<Translation>> Save(Translation model)
    {
      return await Save(model, new TranslationValidator());
    }


    /// <inheritdoc />
    public async Task<EntityResult<Translation>> Delete(string id)
    {
      return await DeleteById<Translation>(id);
    }
  }


  public interface ITranslationsApi : IAppAwareBackofficeApi
  {
    /// <summary>
    /// Get translation by Id
    /// </summary>
    Task<Translation> GetById(string id);

    /// <summary>
    /// Get all available translations
    /// </summary>
    Task<IList<Translation>> GetAll();

    /// <summary>
    /// Get all available translations (with query)
    /// </summary>
    Task<ListResult<Translation>> GetByQuery(ListQuery<Translation> query);

    /// <summary>
    /// Creates or updates a translation
    /// </summary>
    Task<EntityResult<Translation>> Save(Translation model);

    /// <summary>
    /// Deletes a translation by Id
    /// </summary>
    Task<EntityResult<Translation>> Delete(string id);
  }
}
