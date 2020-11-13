using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class MailTemplatesApi : BackofficeApi, IMailTemplatesApi
  {
    protected IValidator<IMailTemplate> Validator { get; private set; }

    public MailTemplatesApi(IBackofficeStore store, IValidator<IMailTemplate> validator) : base(store)
    {
      Validator = validator;
    }



    /// <inheritdoc />
    public async Task<IMailTemplate> GetById(string id)
    {
      return await GetById<IMailTemplate>(id);
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, IMailTemplate>> GetByIds(params string[] ids)
    {
      return await GetByIds<IMailTemplate>(ids);
    }


    /// <summary>
    /// Get all available currencies
    /// </summary>
    public async Task<IList<IMailTemplate>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IMailTemplate>().Scope(Scope).ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<IMailTemplate>> GetByQuery(ListQuery<IMailTemplate> query)
    {
      query.SearchSelector = entity => entity.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IMailTemplate>()
          .Scope(Scope)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<IMailTemplate>> Save(IMailTemplate model)
    {
      return await SaveModel(model, Validator);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IMailTemplate>> Delete(string id)
    {
      return await DeleteById<IMailTemplate>(id);
    }
  }


  public interface IMailTemplatesApi
  {
    /// <summary>
    /// Get mail template by id
    /// </summary>
    Task<IMailTemplate> GetById(string id);

    /// <summary>
    /// Get mail templates by ids
    /// </summary>
    Task<Dictionary<string, IMailTemplate>> GetByIds(params string[] ids);

    /// <summary>
    /// Get all available mail templates
    /// </summary>
    Task<IList<IMailTemplate>> GetAll();

    /// <summary>
    /// Get all available mail templates (with query)
    /// </summary>
    Task<ListResult<IMailTemplate>> GetByQuery(ListQuery<IMailTemplate> query);

    /// <summary>
    /// Creates or updates a mail template
    /// </summary>
    Task<EntityResult<IMailTemplate>> Save(IMailTemplate model);

    /// <summary>
    /// Deletes a mail template by id
    /// </summary>
    Task<EntityResult<IMailTemplate>> Delete(string id);
  }
}
