using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Collections
{
  public class CountriesCollection : CollectionBase<Country>, ICountriesCollection
  {
    public CountriesCollection(ICollectionContext context, IValidator<Country> validator) : base(context, validator) { }


    /// <inheritdoc />
    public override IAsyncEnumerable<Country> Stream()
    {
      return base.Stream(q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name));
    }
  }


  public interface ICountriesCollection : ICollectionBase<Country>
  {

  }
}
