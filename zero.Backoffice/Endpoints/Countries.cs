//using Microsoft.AspNetCore.Mvc;
//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using System.Threading.Tasks;
//using zero;

//namespace zero.Backoffice
//{
//  [ZeroAuthorize(Permissions.Settings.Countries, PermissionsValue.Read)]
//  public class CountriesController : ZeroBackofficeCollectionController<Country, ICountriesCollection>
//  {
//    public CountriesController(ICountriesCollection collection) : base(collection)
//    {
//      PreviewTransform = (item, model) => model.Icon = "flag-" + item.Code.ToLowerInvariant();
//    }

//    public override Task<ListResult<Country>> GetByQuery([FromQuery] ListBackofficeQuery<Country> query)
//    {
//      query.OrderQuery = q => q.OrderByDescending(x => x.IsPreferred).ThenBy(x => x.Name);
//      return Collection.Load<zero_Countries>(query);
//    }
//  }
//}
