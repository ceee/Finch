using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class LanguagesApi<T> : BackofficeApi, ILanguagesApi<T> where T : ILanguage
  {
    IValidator<T> Validator;


    public LanguagesApi(IBackofficeStore store, IValidator<T> validator) : base(store)
    {
      Validator = validator;
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
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public IList<Culture> GetAllCultures(params string[] codes)
    {
      return CultureInfo.GetCultures(CultureTypes.AllCultures)
        .Where(x => !x.Name.IsNullOrWhiteSpace())
        .Select(x => new CultureInfo(x.Name))
        .Where(x => codes.Length > 0 ? codes.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase) : true)
        .OrderBy(x => x.DisplayName)
        .Select(x => new Culture()
        {
          Code = x.Name,
          Name = x.DisplayName
        })
        .ToList();
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    {
      query.SearchFor(entity => entity.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      return await SaveModel(model, Validator);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Delete(string id)
    {
      return await DeleteById<T>(id);
    }
  }


  public interface ILanguagesApi<T> where T : ILanguage
  {
    /// <summary>
    /// Get language by Id
    /// </summary>
    Task<T> GetById(string id);

    /// <summary>
    /// Get all available languages
    /// </summary>
    Task<IList<T>> GetAll();

    /// <summary>
    /// Get all available cultures
    /// </summary>
    IList<Culture> GetAllCultures(params string[] codes);

    /// <summary>
    /// Get all available languages (with query)
    /// </summary>
    Task<ListResult<T>> GetByQuery(ListQuery<T> query);

    /// <summary>
    /// Creates or updates a language
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Deletes a language by Id
    /// </summary>
    Task<EntityResult<T>> Delete(string id);
  }
}
