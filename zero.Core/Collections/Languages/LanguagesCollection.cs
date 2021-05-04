using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Collections
{
  public class LanguagesCollection : CollectionBase<Language>, ILanguagesCollection
  {
    public LanguagesCollection(IZeroContext context, ICollectionInterceptorHandler interceptor, IValidator<Language> validator) : base(context, interceptor, validator) { }


    /// <inheritdoc />
    public override IAsyncEnumerable<Language> Stream()
    {
      return base.Stream(q => q.OrderByDescending(x => x.CreatedDate));
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
  }


  public interface ILanguagesCollection : ICollectionBase<Language>
  {
    /// <summary>
    /// Get all available cultures
    /// </summary>
    IList<Culture> GetAllCultures(params string[] codes);
  }
}
