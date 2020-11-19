using FluentValidation;
using Raven.Client.Documents;
using System.Threading.Tasks;
using zero.Core.Entities;

namespace zero.Core.Collections
{
  public class MailTemplatesCollection : CollectionBase<IMailTemplate>, IMailTemplatesCollection
  {
    public MailTemplatesCollection(IZeroContext context, ICollectionInterceptorHandler interceptor, IValidator<IMailTemplate> validator) : base(context, interceptor, validator) { }


    /// <inheritdoc />
    public async Task<IMailTemplate> GetByKey(string key)
    {
      return await Query.FirstOrDefaultAsync(x => x.Key == key);
    }
  }


  public interface IMailTemplatesCollection : ICollectionBase<IMailTemplate>
  {
    /// <summary>
    /// Get mail template by associated key
    /// </summary>
    Task<IMailTemplate> GetByKey(string key);
  }
}
