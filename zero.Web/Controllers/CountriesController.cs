using Raven.Client.Documents.Linq;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
  public class CountriesController : BackofficeCollectionController<Country, ICountriesCollection>
  {
    public CountriesController(ICountriesCollection collection) : base(collection)
    {
      DefaultQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
      PreviewTransform = (item, model) => model.Icon = "flag-" + item.Code.ToLowerInvariant();
    }
  }
}
