using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Collections
{
  public class CountriesCollection : CollectionBase<ICountry>, ICountriesCollection
  {
    public CountriesCollection(IZeroContext context, ICollectionInterceptorHandler interceptor, IValidator<ICountry> validator) : base(context, interceptor, validator) { }


    /// <inheritdoc />
    public override IAsyncEnumerable<ICountry> Stream()
    {
      return base.Stream(q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name));
    }
  }


  public interface ICountriesCollection : ICollectionBase<ICountry>
  {

  }
}
