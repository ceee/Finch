using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController : ZeroBackofficeCollectionController<Country, ICountriesCollection>
  {
    public CountriesController(ICountriesCollection collection) : base(collection)
    {
      PreviewTransform = (item, model) => model.Icon = "flag-" + item.Code.ToLowerInvariant();
    }

    public override Task<ListResult<Country>> GetByQuery([FromQuery] ListBackofficeQuery<Country> query)
    {
      query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
      return Collection.Load<zero_Countries>(query);
    }
  }
}
