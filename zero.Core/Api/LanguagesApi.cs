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
  public class LanguagesApi : BackofficeApi, ILanguagesApi
  {
    IValidator<ILanguage> Validator;


    public LanguagesApi(IBackofficeStore store, IValidator<ILanguage> validator) : base(store)
    {
      Validator = validator;
    }


    /// <inheritdoc />
    public async Task<ILanguage> GetById(string id)
    {
      return await GetById<ILanguage>(id);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, ILanguage>> GetByIds(params string[] ids)
    {
      return await GetByIds<ILanguage>(ids);
    }


    /// <inheritdoc />
    public async Task<IList<ILanguage>> GetAll()
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      return await session.Query<ILanguage>()
        .OrderByDescending(x => x.CreatedDate)
        .ToListAsync();
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
    public async Task<ListResult<ILanguage>> GetByQuery(ListQuery<ILanguage> query)
    {
      query.SearchFor(entity => entity.Name);

      using IAsyncDocumentSession session = Store.OpenAsyncSession();

      return await session.Query<ILanguage>().ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ILanguage>> Save(ILanguage model)
    {
      return await SaveModel(model, Validator);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ILanguage>> Delete(string id)
    {
      return await DeleteById<ILanguage>(id);
    }
  }


  public interface ILanguagesApi
  {
    /// <summary>
    /// Get language by Id
    /// </summary>
    Task<ILanguage> GetById(string id);

    /// <summary>
    /// Get countries by ids
    /// </summary>
    Task<Dictionary<string, ILanguage>> GetByIds(params string[] ids);

    /// <summary>
    /// Get all available languages
    /// </summary>
    Task<IList<ILanguage>> GetAll();

    /// <summary>
    /// Get all available cultures
    /// </summary>
    IList<Culture> GetAllCultures(params string[] codes);

    /// <summary>
    /// Get all available languages (with query)
    /// </summary>
    Task<ListResult<ILanguage>> GetByQuery(ListQuery<ILanguage> query);

    /// <summary>
    /// Creates or updates a language
    /// </summary>
    Task<EntityResult<ILanguage>> Save(ILanguage model);

    /// <summary>
    /// Deletes a language by Id
    /// </summary>
    Task<EntityResult<ILanguage>> Delete(string id);
  }
}
