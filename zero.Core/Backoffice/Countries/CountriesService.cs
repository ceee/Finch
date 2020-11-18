using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Backoffice
{
  public class CountriesBackofficeService : BackofficeService<ICountry>, ICountriesBackofficeService
  {
    public CountriesBackofficeService(IZeroContext context, IValidator<ICountry> validator) : base(context, validator) { }


    /// <inheritdoc />
    public override IAsyncEnumerable<ICountry> Stream()
    {
      return base.Stream(q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name));
    }


    /// <inheritdoc />
    //public async Task<ListResult<ICountry>> GetByQuery(string languageId, ListQuery<ICountry> query)
    //{
    //  query.SearchSelector = country => country.Name;

    //  using IAsyncDocumentSession session = Store.OpenAsyncSession();
    //  return await session.Query<ICountry>()
    //    .OrderByDescending(x => x.IsPreferred)
    //    .ThenBy(x => x.Name)
    //    .ToQueriedListAsync(query);
    //}
  }


  public interface ICountriesBackofficeService : IBackofficeService<ICountry>
  {

  }
}
