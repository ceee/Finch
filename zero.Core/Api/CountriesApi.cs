using FluentValidation;
using FluentValidation.Results;
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
  public class CountriesApi<T> : BackofficeApi, ICountriesApi<T> where T : ICountry
  {
    IValidator<T> Validator;


    public CountriesApi(IBackofficeStore store, IValidator<T> validator) : base(store)
    {
      Validator = validator;
    }


    /// <inheritdoc />
    public async Task<T> GetById(string id)
    {
      return await GetById<T>(id);
    }


    /// <inheritdoc />
    public async Task<IList<T>> GetAll(string languageId)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .Where(x => x.LanguageId == languageId)
          .OrderByDescending(x => x.IsPreferred)
          .ThenBy(x => x.Name)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetByQuery(string languageId, ListQuery<T> query)
    {
      query.SearchSelector = country => country.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .Where(x => x.LanguageId == languageId)
          .OrderByDescending(x => x.IsPreferred)
          .ThenBy(x => x.Name)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      ValidationResult validation = await Validator.ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<T>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.Alias = Alias.Generate(model.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(model);

        string id = session.Advanced.GetDocumentId(model);

        await session.SaveChangesAsync();

        if (model.Id.IsNullOrEmpty())
        {
          model.Id = id;
        }
      }

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Delete(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        T country = await session.LoadAsync<T>(id);

        if (country == null)
        {
          return EntityResult<T>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(country);

        await session.SaveChangesAsync();
      }

      return EntityResult<T>.Success();
    }
  }


  public interface ICountriesApi<T> where T : ICountry
  {
    /// <summary>
    /// Get country by Id
    /// </summary>
    Task<T> GetById(string id);

    /// <summary>
    /// Get all available countries
    /// </summary>
    Task<IList<T>> GetAll(string languageId);

    /// <summary>
    /// Get all available countries (with query)
    /// </summary>
    Task<ListResult<T>> GetByQuery(string languageId, ListQuery<T> query);

    /// <summary>
    /// Creates or updates a country
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Deletes a country by Id
    /// </summary>
    Task<EntityResult<T>> Delete(string id);
  }
}
