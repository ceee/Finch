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
  }


  public interface ICountriesBackofficeService : IBackofficeService<ICountry>
  {

  }
}
