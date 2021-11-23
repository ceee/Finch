using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Mails, PermissionsValue.Read)]
  public class MailTemplatesController : ZeroBackofficeCollectionController<MailTemplate, IMailTemplatesStore>
  {
    public MailTemplatesController(IMailTemplatesStore collection) : base(collection)
    {
      PreviewTransform = (item, model) => model.Icon = "fth-mail";
    }

    public override async Task<Paged<MailTemplate>> GetByQuery([FromQuery] ListBackofficeQuery<MailTemplate> query)
    {
      query.SearchFor(entity => entity.Name, entity => entity.Key, entity => entity.Subject);
      return await Collection.Load<zero_MailTemplates>(query);
    }
  }
}
