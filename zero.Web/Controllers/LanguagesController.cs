using Raven.Client.Documents.Linq;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Read)]
  public class LanguagesController : BackofficeCollectionController<ILanguage, ILanguagesCollection>
  {
    public LanguagesController(ILanguagesCollection collection) : base(collection)
    {
      DefaultQuery = q => q.OrderByDescending(x => x.CreatedDate);
      PreviewTransform = (item, model) => model.Icon = "fth-globe";
    }
  }
}
