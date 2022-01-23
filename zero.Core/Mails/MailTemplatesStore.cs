using FluentValidation.Results;
using Raven.Client.Documents;

namespace zero.Mails;

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
  public override Task<ValidationResult> Validate(ZeroValidationContext ctx, MailTemplate model)
  {
    return new MailTemplateValidator(ctx, Dispatcher).ValidateAsync(model);
  }
}


public interface IMailTemplatesStore : IEntityStore<MailTemplate>
{
  /// <summary>
  /// Get mail template by associated key
  /// </summary>
  Task<MailTemplate> GetByKey(string key);
}