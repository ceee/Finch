using FluentValidation;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using Raven.Client.Documents.Session;
using Raven.Client.Documents;

namespace zero.Core.Validation
{
  public class CountryValidator : ZeroValidator<ICountry, Country>
  {
    public CountryValidator(IBackofficeStore store) : base(store)
    {
      RuleFor(x => x.Code).Length(2);

      RuleFor(x => x.Code).Unique(store);

      //RuleFor(x => x.Code).Query<ICountry, Country>(store, async (query, entity) =>
      //{
      //  return !(await query.Scope(store.AppContext.AppId, true).AnyAsync(x => x.Id != entity.Id && x.Code == value));
      //});

      //RuleFor(x => x.Code).MustAsync(async (entity, value, ct) =>
      //{
      //  using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      //  return !(await session.Query<ICountry>().Scope(store.AppContext.AppId, true).AnyAsync(x => x.Id != entity.Id && x.Code == value));
      //});

      RuleFor(x => x.Name).Length(2, 120);
    }
  }
}
