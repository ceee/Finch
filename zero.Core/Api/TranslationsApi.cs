using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class TranslationsApi : AppAwareBackofficeApi, ITranslationsApi
  {
    //IValidator<ITranslation> Validator;


    public TranslationsApi(IBackofficeStore store) : base(store)
    {
      Scope.IncludeShared = true;
    }


    /// <inheritdoc />
    public async Task<string> GetStringById(string id)
    {
      return (await GetById<ITranslation>(id))?.Value;
    }


    /// <inheritdoc />
    public async Task<ITranslation> GetById(string id)
    {
      return await GetById<ITranslation>(id);
    }


    /// <inheritdoc />
    public async Task<IList<ITranslation>> GetAll()
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await session.Query<ITranslation>().OrderByDescending(x => x.CreatedDate).Scope(Scope).ToListAsync();
    }


    /// <inheritdoc />
    public async Task<ListResult<ITranslation>> GetByQuery(ListQuery<ITranslation> query)
    {
      query.SearchFor(entity => entity.Key, entity => entity.Value);

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await session.Query<ITranslation>().OrderByDescending(x => x.CreatedDate).Scope(Scope).ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ITranslation>> Save(ITranslation model)
    {
      return await SaveModel(model, null);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ITranslation>> Delete(string id)
    {
      return await DeleteById<ITranslation>(id);
    }
  }


  public interface ITranslationsApi
  {
    /// <summary>
    /// Get translation by id
    /// </summary>
    Task<ITranslation> GetById(string id);

    /// <summary>
    /// Get a translated string by id
    /// </summary>
    Task<string> GetStringById(string id);

    /// <summary>
    /// Get all available translations
    /// </summary>
    Task<IList<ITranslation>> GetAll();

    /// <summary>
    /// Get all available translations (with query)
    /// </summary>
    Task<ListResult<ITranslation>> GetByQuery(ListQuery<ITranslation> query);

    /// <summary>
    /// Creates or updates a translation
    /// </summary>
    Task<EntityResult<ITranslation>> Save(ITranslation model);

    /// <summary>
    /// Deletes a translation by id
    /// </summary>
    Task<EntityResult<ITranslation>> Delete(string id);
  }
}
