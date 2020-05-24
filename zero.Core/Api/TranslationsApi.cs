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
  public class TranslationsApiFacade : ITranslationsApiFacade
  {
    IServiceProvider Services;

    public TranslationsApiFacade(IServiceProvider services)
    {
      Services = services;
    }

    public ITranslationsApi<T> As<T>() where T : ITranslation
    {
      return Services.GetService(typeof(ITranslationsApi<T>)) as ITranslationsApi<T>;
    }
  }


  public interface ITranslationsApiFacade
  {
    ITranslationsApi<T> As<T>() where T : ITranslation;
  }


  public class TranslationsApi : TranslationsApi<ITranslation>, ITranslationsApi
  {
    public TranslationsApi(IBackofficeStore store) : base(store) { }
  }


  public class TranslationsApi<T> : AppAwareBackofficeApi, ITranslationsApi<T> where T : ITranslation
  {
    IValidator<T> Validator;


    public TranslationsApi(IBackofficeStore store) : base(store)
    {
      //Validator = validator;
      AllowShared = true;
    }


    /// <inheritdoc />
    public async Task<T> GetById(string id)
    {
      return await GetById<T>(id);
    }


    /// <inheritdoc />
    public async Task<IList<T>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .OrderByDescending(x => x.CreatedDate)
          .Scope(Scope)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    {
      query.SearchFor(entity => entity.Key, entity => entity.Value);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>().Scope(Scope).ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      return await Save(model, Validator);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Delete(string id)
    {
      return await DeleteById<T>(id);
    }
  }


  public interface ITranslationsApi<T> where T : ITranslation
  {
    /// <summary>
    /// Get translation by Id
    /// </summary>
    Task<T> GetById(string id);

    /// <summary>
    /// Get all available translations
    /// </summary>
    Task<IList<T>> GetAll();

    /// <summary>
    /// Get all available translations (with query)
    /// </summary>
    Task<ListResult<T>> GetByQuery(ListQuery<T> query);

    /// <summary>
    /// Creates or updates a translation
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Deletes a translation by Id
    /// </summary>
    Task<EntityResult<T>> Delete(string id);
  }


  public interface ITranslationsApi : ITranslationsApi<ITranslation>
  {

  }
}
