using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Languages, PermissionsValue.Read)]
  public class LanguagesController : ZeroBackofficeCollectionController<Language, ILanguagesCollection>
  {
    public LanguagesController(ILanguagesCollection collection) : base(collection)
    {
      PreviewTransform = (item, model) => model.Icon = "fth-globe";
    }


    public List<Culture> GetAllCultures()
    {
      return Collection.GetAllCultures();
    }


    public List<Culture> GetSupportedCultures()
    {
      return Collection.GetAllCultures(Options.SupportedLanguages);
    }


    public override async Task<Paged<Language>> GetByQuery([FromQuery] ListBackofficeQuery<Language> query)
    {
      query.OrderQuery = q => q.OrderByDescending(x => x.CreatedDate);
      return await Collection.Load<zero_Languages>(query);
    }
  }
}
