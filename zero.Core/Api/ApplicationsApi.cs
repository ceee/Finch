using FluentValidation;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class ApplicationsApi<T> : BackofficeApi, IApplicationsApi<T> where T : IApplication
  {
    IValidator<T> Validator;


    public ApplicationsApi(IBackofficeStore store, IValidator<T> validator) : base(store)
    {
      Validator = validator;
    }


    /// <inheritdoc />
    public async Task<T> GetById(string id)
    {
      return await GetById<T>(id);
    }


    /// <inheritdoc />
    public async Task<IList<T>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session
          .Query<T>()
          .OrderByDescending(x => x.CreatedDate)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetByQuery(ListQuery<T> query)
    {
      query.SearchFor(entity => entity.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      return await SaveModel(model, Validator);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Delete(string id)
    {
      return await DeleteById<T>(id);
    }
  }


  public interface IApplicationsApi<T> where T : IApplication
  {
    /// <summary>
    /// Get application by Id
    /// </summary>
    Task<T> GetById(string id);

    /// <summary>
    /// Get all available zero applications
    /// </summary>
    Task<IList<T>> GetAll();

    /// <summary>
    /// Get all available applications (with query)
    /// </summary>
    Task<ListResult<T>> GetByQuery(ListQuery<T> query);

    /// <summary>
    /// Creates or updates a application
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Deletes a application by Id
    /// </summary>
    Task<EntityResult<T>> Delete(string id);
  }
}
