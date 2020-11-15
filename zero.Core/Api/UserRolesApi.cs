using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;
using zero.Core.Validation;

namespace zero.Core.Api
{
  public class UserRolesApi : BackofficeApi, IUserRolesApi
  {
    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected UserManager<BackofficeUser> UserManager { get; private set; }

    protected RoleManager<BackofficeUserRole> RoleManager { get; private set; }

    private ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public UserRolesApi(IHttpContextAccessor httpContextAccessor, UserManager<BackofficeUser> userManager, RoleManager<BackofficeUserRole> roleManager, IBackofficeStore store) : base(store, isCoreDatabase: true)
    {
      HttpContextAccessor = httpContextAccessor;
      UserManager = userManager;
      RoleManager = roleManager;
    }


    /// <inheritdoc />
    public async Task<IList<IBackofficeUserRole>> GetAll()
    {
      using IAsyncDocumentSession session = Session();
      return await session.Query<IBackofficeUserRole>().OrderBy(x => x.Sort).ThenBy(x => x.Name).ToListAsync();
    }


    /// <inheritdoc />
    public async Task<IBackofficeUserRole> GetById(string id)
    {
      return await RoleManager.FindByIdAsync(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IBackofficeUserRole>> Save(IBackofficeUserRole model)
    {
      ValidationResult validation = await new UserRoleValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<IBackofficeUserRole>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.Alias = Safenames.Alias(model.Name);

      using (IAsyncDocumentSession session = Session())
      {
        await session.StoreAsync(model);

        string id = session.Advanced.GetDocumentId(model);

        await session.SaveChangesAsync();

        if (model.Id.IsNullOrEmpty())
        {
          model.Id = id;
        }
      }

      return EntityResult<IBackofficeUserRole>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IBackofficeUserRole>> Delete(string id)
    {
      using (IAsyncDocumentSession session = Session())
      {
        BackofficeUserRole country = await session.LoadAsync<BackofficeUserRole>(id);

        if (country == null)
        {
          return EntityResult<IBackofficeUserRole>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(country);

        await session.SaveChangesAsync();
      }

      return EntityResult<IBackofficeUserRole>.Success();
    }
  }


  public interface IUserRolesApi : IBackofficeApi
  {
    /// <summary>
    /// Get all user roles
    /// </summary>
    Task<IList<IBackofficeUserRole>> GetAll();

    /// <summary>
    /// Get role by id
    /// </summary>
    Task<IBackofficeUserRole> GetById(string id);

    /// <summary>
    /// Create or update a role
    /// </summary>
    Task<EntityResult<IBackofficeUserRole>> Save(IBackofficeUserRole model);

    /// <summary>
    /// Deletes a role
    /// </summary>
    Task<EntityResult<IBackofficeUserRole>> Delete(string id);
  }
}
