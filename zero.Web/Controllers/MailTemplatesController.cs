using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Database.Indexes;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Mails, PermissionsValue.Read)]
  public class MailTemplatesController : BackofficeCollectionController<MailTemplate, IMailTemplatesCollection>
  {
    public MailTemplatesController(IMailTemplatesCollection collection) : base(collection)
    {
      PreviewTransform = (item, model) => model.Icon = "fth-mail";
    }

    public override async Task<ListResult<MailTemplate>> GetByQuery([FromQuery] ListBackofficeQuery<MailTemplate> query)
    {
      query.SearchFor(entity => entity.Name, entity => entity.Key, entity => entity.Subject);
      return await Collection.GetByQuery<zero_MailTemplates>(query);
    }
  }
}
