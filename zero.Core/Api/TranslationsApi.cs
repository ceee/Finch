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
  public class TranslationsApi : BackofficeApi<ITranslationsApi>, ITranslationsApi
  {
    public TranslationsApi(IDocumentStore raven, IApplicationContext appContext) : base(raven)
    {
      AppId = appContext.AppId;
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
          .ForApp(AppId)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<Translation>> GetByQuery(ListQuery<Translation> query)
    {
      query.SearchFor(entity => entity.Key, entity => entity.Value);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Translation>().ForApp(AppId).ToQueriedListAsync(query);
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


  public interface ITranslationsApi : IBackofficeApi<ITranslationsApi>
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
