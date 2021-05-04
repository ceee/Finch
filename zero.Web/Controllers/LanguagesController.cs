using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Read)]
  public class LanguagesController : BackofficeCollectionController<Language, ILanguagesCollection>
  {
    public LanguagesController(ILanguagesCollection collection) : base(collection)
    {
      DefaultQuery = q => q.OrderByDescending(x => x.CreatedDate);
      PreviewTransform = (item, model) => model.Icon = "fth-globe";
    }


    public IList<Culture> GetAllCultures()
    {
      return Collection.GetAllCultures();
    }


    public IList<Culture> GetSupportedCultures()
    {
      return Collection.GetAllCultures(Options.SupportedLanguages);
    }
  }
}
