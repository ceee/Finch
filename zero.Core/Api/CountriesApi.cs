using FluentValidation.Results;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class CountriesApi : ICountriesApi
  {
    protected IDocumentStore Raven { get; private set; }


    public CountriesApi(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public async Task<Country> GetById(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<Country>(id);
      }
    }


    /// <inheritdoc />
    public async Task<IList<Country>> GetAll(string languageId)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Country>()
          .Where(x => x.LanguageId == languageId)
          .OrderByDescending(x => x.IsPreferred)
          .ThenBy(x => x.Name)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<Country>> GetByQuery(string languageId, ListQuery<Country> query)
    {
      query.SearchSelector = country => country.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Country>()
          .Where(x => x.LanguageId == languageId)
          .OrderByDescending(x => x.IsPreferred)
          .ThenBy(x => x.Name)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<Country>> Save(Country model)
    {
      ValidationResult validation = await new CountryValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<Country>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.AppId = "zero.applications.1-A"; // TODO real app id
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

      return EntityResult<Country>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Country>> Delete(string id)
    {
      return EntityResult<Country>.Success();

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        Country country = await session.LoadAsync<Country>(id);

        if (country == null)
        {
          return EntityResult<Country>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(country);

        await session.SaveChangesAsync();
      }

      return EntityResult<Country>.Success();
    }
  }


  public interface ICountriesApi
  {
    /// <summary>
    /// Get country by Id
    /// </summary>
    Task<Country> GetById(string id);

    /// <summary>
    /// Get all available countries
    /// </summary>
    Task<IList<Country>> GetAll(string languageId);

    /// <summary>
    /// Get all available countries (with query)
    /// </summary>
    Task<ListResult<Country>> GetByQuery(string languageId, ListQuery<Country> query);

    /// <summary>
    /// Creates or updates a country
    /// </summary>
    Task<EntityResult<Country>> Save(Country model);

    /// <summary>
    /// Deletes a country by Id
    /// </summary>
    Task<EntityResult<Country>> Delete(string id);
  }
}
