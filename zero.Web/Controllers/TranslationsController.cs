using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Linq;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Translations, PermissionsValue.Read)]
  public class TranslationsController : BackofficeCollectionController<Translation, ITranslationsCollection>
  {
    public TranslationsController(ITranslationsCollection collection) : base(collection) { }

    public override async Task<ListResult<Translation>> GetByQuery([FromQuery] ListBackofficeQuery<Translation> query)
    {
      query.SearchFor(entity => entity.Key, entity => entity.Value);
      query.OrderQuery = q => q.OrderByDescending(x => x.CreatedDate);
      return await Collection.GetByQuery<zero_Translations>(query);
    }
  }
}
