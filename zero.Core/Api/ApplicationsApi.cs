using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class ApplicationsApi : BackofficeApi, IApplicationsApi
  {
    IValidator<IApplication> Validator;


    public ApplicationsApi(IBackofficeStore store, IValidator<IApplication> validator) : base(store)
    {
      Validator = validator;
    }


    /// <inheritdoc />
    public async Task<IApplication> GetById(string id)
    {
      return await GetById<IApplication>(id);
    }


    /// <inheritdoc />
    public async Task<IList<IApplication>> GetAll()
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await session
        .Query<IApplication>()
        .OrderByDescending(x => x.CreatedDate)
        .ToListAsync();
    }


    /// <inheritdoc />
    public async Task<ListResult<IApplication>> GetByQuery(ListQuery<IApplication> query)
    {
      query.SearchFor(entity => entity.Name);

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await session.Query<IApplication>().ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IApplication>> Save(IApplication model)
    {
      return await SaveModel(model, Validator);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IApplication>> Delete(string id)
    {
      return await DeleteById<IApplication>(id);
    }
  }


  public interface IApplicationsApi
  {
    /// <summary>
    /// Get application by Id
    /// </summary>
    Task<IApplication> GetById(string id);

    /// <summary>
    /// Get all available zero applications
    /// </summary>
    Task<IList<IApplication>> GetAll();

    /// <summary>
    /// Get all available applications (with query)
    /// </summary>
    Task<ListResult<IApplication>> GetByQuery(ListQuery<IApplication> query);

    /// <summary>
    /// Creates or updates a application
    /// </summary>
    Task<EntityResult<IApplication>> Save(IApplication model);

    /// <summary>
    /// Deletes a application by Id
    /// </summary>
    Task<EntityResult<IApplication>> Delete(string id);
  }
}
