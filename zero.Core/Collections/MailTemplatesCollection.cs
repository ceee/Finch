using FluentValidation;
using Raven.Client.Documents;

namespace zero.Collections;

public class MailTemplatesCollection : EntityCollection<MailTemplate>, IMailTemplatesCollection
{
  protected IMailDispatcher Dispatcher { get; set; }


  public MailTemplatesCollection(ICollectionContext context, IMailDispatcher dispatcher = null) : base(context)
  {
    Dispatcher = dispatcher;
  }


  /// <inheritdoc />
  public async Task<MailTemplate> GetByKey(string key)
  {
    return await Session.Query<MailTemplate>().FirstOrDefaultAsync(x => x.Key == key);
  }


  /// <inheritdoc />
  protected override void ValidationRules(ZeroValidator<MailTemplate> validator)
  {
    validator.RuleFor(x => x.SenderEmail).Email();

    if (Dispatcher != null)
    {
      validator.RuleFor(x => x.SenderEmail).MustAsync(async (value, ct) =>
      {
        return await Dispatcher.IsSenderSupported(value);
      }).WithMessage("@mailTemplate.errors.senderNotAllowed");
    }
  }
}


public interface IMailTemplatesCollection : IEntityCollection<MailTemplate>
{
  /// <summary>
  /// Get mail template by associated key
  /// </summary>
  Task<MailTemplate> GetByKey(string key);
}