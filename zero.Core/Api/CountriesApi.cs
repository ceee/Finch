using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class CountriesApi : BackofficeApi, ICountriesApi
  {
    IValidator<ICountry> Validator;


    public CountriesApi(IBackofficeStore store, IValidator<ICountry> validator) : base(store)
    {
      Validator = validator;
    }


    /// <inheritdoc />
    public async Task<ICountry> GetById(string id)
    {
      return await GetById<ICountry>(id);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, ICountry>> GetByIds(params string[] ids)
    {
      return await GetByIds<ICountry>(ids);
    }


    /// <inheritdoc />
    public async Task<IList<ICountry>> GetAll(string languageId)
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await session.Query<ICountry>()
        .OrderByDescending(x => x.IsPreferred)
        .ThenBy(x => x.Name)
        .ToListAsync();
    }


    /// <inheritdoc />
    public async Task<ListResult<ICountry>> GetByQuery(string languageId, ListQuery<ICountry> query)
    {
      query.SearchSelector = country => country.Name;

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await session.Query<ICountry>()
        .OrderByDescending(x => x.IsPreferred)
        .ThenBy(x => x.Name)
        .ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ICountry>> Save(ICountry model)
    {
      return await SaveModel(model, Validator);
    }


    /// <inheritdoc />
    public async Task<EntityResult<ICountry>> Delete(string id)
    {
      return await DeleteById<ICountry>(id);
    }
  }


  public interface ICountriesApi
  {
    /// <summary>
    /// Get country by Id
    /// </summary>
    Task<ICountry> GetById(string id);

    /// <summary>
    /// Get countries by ids
    /// </summary>
    Task<Dictionary<string, ICountry>> GetByIds(params string[] ids);

    /// <summary>
    /// Get all available countries
    /// </summary>
    Task<IList<ICountry>> GetAll(string languageId);

    /// <summary>
    /// Get all available countries (with query)
    /// </summary>
    Task<ListResult<ICountry>> GetByQuery(string languageId, ListQuery<ICountry> query);

    /// <summary>
    /// Creates or updates a country
    /// </summary>
    Task<EntityResult<ICountry>> Save(ICountry model);

    /// <summary>
    /// Deletes a country by Id
    /// </summary>
    Task<EntityResult<ICountry>> Delete(string id);
  }
}
