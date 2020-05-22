using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class ApplicationsApi : BackofficeApi, IApplicationsApi
  {
    public ApplicationsApi(IBackofficeStore store) : base(store)
    {
      
    }


    /// <inheritdoc />
    public async Task<Application> GetById(string id)
    {
      return await GetById<Application>(id);
    }


    /// <inheritdoc />
    public async Task<IList<Application>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session
          .Query<Application>()
          .OrderByDescending(x => x.CreatedDate)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<Application>> GetByQuery(ListQuery<Application> query)
    {
      query.SearchFor(entity => entity.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<Application>()
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<Application>> Save(Application model)
    {
      return await Save(model, new ApplicationValidator());
    }


    /// <inheritdoc />
    public async Task<EntityResult<Application>> Delete(string id)
    {
      return await DeleteById<Application>(id);
    }
  }


  public interface IApplicationsApi
  {
    /// <summary>
    /// Get application by Id
    /// </summary>
    Task<Application> GetById(string id);

    /// <summary>
    /// Get all available zero applications
    /// </summary>
    Task<IList<Application>> GetAll();

    /// <summary>
    /// Get all available applications (with query)
    /// </summary>
    Task<ListResult<Application>> GetByQuery(ListQuery<Application> query);

    /// <summary>
    /// Creates or updates a application
    /// </summary>
    Task<EntityResult<Application>> Save(Application model);

    /// <summary>
    /// Deletes a application by Id
    /// </summary>
    Task<EntityResult<Application>> Delete(string id);
  }
}
