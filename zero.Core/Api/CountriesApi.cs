using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;

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
  }


  public interface ICountriesApi
  {
    /// <summary>
    /// Get all available countries
    /// </summary>
    Task<IList<Country>> GetAll(string languageId);
  }
}
