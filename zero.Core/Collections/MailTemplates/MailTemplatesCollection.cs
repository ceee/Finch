using FluentValidation;
using Raven.Client.Documents;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public class MailTemplatesCollection : CollectionBase<MailTemplate>, IMailTemplatesCollection
  {
    public MailTemplatesCollection(ICollectionContext context, IValidator<MailTemplate> validator) : base(context, validator) { }


    /// <inheritdoc />
    public async Task<MailTemplate> GetByKey(string key)
    {
      return await Query.FirstOrDefaultAsync(x => x.Key == key);
    }
  }


  public interface IMailTemplatesCollection : ICollectionBase<MailTemplate>
  {
    /// <summary>
    /// Get mail template by associated key
    /// </summary>
    Task<MailTemplate> GetByKey(string key);
  }
}
