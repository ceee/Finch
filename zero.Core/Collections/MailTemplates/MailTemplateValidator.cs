using FluentValidation;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Mails;
using zero.Core.Validation;

namespace zero.Core.Collections
{
  public class MailTemplateValidator : ZeroValidator<MailTemplate>
  {
    public MailTemplateValidator(IMailDispatcher dispatcher = null)
    {
      RuleFor(x => x.SenderEmail).Email();

      if (dispatcher != null)
      {
        RuleFor(x => x.SenderEmail).MustAsync(async (value, ct) =>
        {
          return await dispatcher.IsSenderSupported(value);
        }).WithMessage("@mailTemplate.errors.senderNotAllowed");
      }
    }
  }
}
