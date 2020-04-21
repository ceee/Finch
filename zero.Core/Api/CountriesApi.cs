using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

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
  }
}
