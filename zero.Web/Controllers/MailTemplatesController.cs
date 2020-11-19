using zero.Core.Collections;
using zero.Core.Entities;
using zero.Core.Identity;

namespace zero.Web.Controllers
{
  [ZeroAuthorize(Permissions.Settings.Mails, PermissionsValue.Read)]
  public class MailTemplatesController : BackofficeCollectionController<IMailTemplate, IMailTemplatesCollection>
  {
    public MailTemplatesController(IMailTemplatesCollection collection) : base(collection)
    {
      PreviewTransform = (item, model) => model.Icon = "fth-mail";
    }
  }
}
