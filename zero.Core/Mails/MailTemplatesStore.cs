using Raven.Client.Documents;

namespace zero.Mails;

public class MailTemplatesStore : EntityStore<MailTemplate>, IMailTemplatesStore
{
  public MailTemplatesStore(IStoreContext context) : base(context) { }


  /// <inheritdoc />
  public async Task<MailTemplate> GetByKey(string key)
  {
    return await Session.Query<MailTemplate>().FirstOrDefaultAsync(x => x.Key == key);
  }
}


public interface IMailTemplatesStore : IEntityStore<MailTemplate>
{
  /// <summary>
  /// Get mail template by associated key
  /// </summary>
  Task<MailTemplate> GetByKey(string key);
}