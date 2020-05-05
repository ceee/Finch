using FluentValidation.Results;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class ApplicationsApi : IApplicationsApi
  {
    protected IDocumentStore Raven { get; private set; }


    public ApplicationsApi(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public async Task<Application> GetById(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<Application>(id);
      }
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
      ValidationResult validation = await new ApplicationValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<Application>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.AppId = Constants.Database.SharedAppId;
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.Alias = Alias.Generate(model.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(model);
        await session.SaveChangesAsync();
      }

      return EntityResult<Application>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<Application>> Delete(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        Application entity = await session.LoadAsync<Application>(id);

        if (entity == null)
        {
          return EntityResult<Application>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(entity);

        await session.SaveChangesAsync();
      }

      return EntityResult<Application>.Success();
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
