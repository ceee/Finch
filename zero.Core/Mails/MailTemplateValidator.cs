using FluentValidation;

namespace zero.Mails;

public class MailTemplateValidator : ZeroValidator<MailTemplate>
{
  public MailTemplateValidator(IZeroStore store, IMailDispatcher dispatcher = null)
  {
    RuleFor(x => x.Name).NotEmpty().Length(2, 80);
    RuleFor(x => x.Key).NotEmpty().Unique(store);
    RuleFor(x => x.Subject).NotEmpty().Length(2, 120);
    RuleFor(x => x.Preheader).MaximumLength(240);

    RuleFor(x => x.SenderEmail).Email();
    RuleFor(x => x.SenderName).MaximumLength(120);

    RuleFor(x => x.RecipientEmail).Emails();
    RuleFor(x => x.Cc).Emails();
    RuleFor(x => x.Bcc).Emails();

    if (dispatcher != null)
    {
      RuleFor(x => x.SenderEmail).MustAsync(async (value, ct) =>
      {
        return await dispatcher.IsSenderSupported(value);
      }).WithMessage("@mailTemplate.errors.senderNotAllowed");
    }
  }
}