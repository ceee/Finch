using FluentValidation;
using Raven.Client.Documents;

namespace zero.Stores;

public class MailTemplatesStore : EntityStore<MailTemplate>, IMailTemplatesStore
{
  protected IMailDispatcher Dispatcher { get; set; }


  public MailTemplatesStore(IStoreContext context, IMailDispatcher dispatcher = null) : base(context)
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


public interface IMailTemplatesStore : IEntityStore<MailTemplate>
{
  /// <summary>
  /// Get mail template by associated key
  /// </summary>
  Task<MailTemplate> GetByKey(string key);
}