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
  public class UserRolesApi : AppAwareBackofficeApi, IUserRolesApi
  {
    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected UserManager<User> UserManager { get; private set; }

    protected RoleManager<UserRole> RoleManager { get; private set; }

    private ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public UserRolesApi(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, RoleManager<UserRole> roleManager, IBackofficeStore store) : base(store)
    {
      HttpContextAccessor = httpContextAccessor;
      UserManager = userManager;
      RoleManager = roleManager;
    }


    /// <inheritdoc />
    public async Task<IList<IUserRole>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IUserRole>().OrderBy(x => x.Sort).ThenBy(x => x.Name).ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<IUserRole> GetById(string id)
    {
      return await RoleManager.FindByIdAsync(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IUserRole>> Save(IUserRole model)
    {
      ValidationResult validation = await new UserRoleValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<IUserRole>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.AppId = Scope.AppId;
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.Alias = Safenames.Alias(model.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(model);

        string id = session.Advanced.GetDocumentId(model);

        await session.SaveChangesAsync();

        if (model.Id.IsNullOrEmpty())
        {
          model.Id = id;
        }
      }

      return EntityResult<IUserRole>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IUserRole>> Delete(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        UserRole country = await session.LoadAsync<UserRole>(id);

        if (country == null)
        {
          return EntityResult<IUserRole>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(country);

        await session.SaveChangesAsync();
      }

      return EntityResult<IUserRole>.Success();
    }
  }


  public interface IUserRolesApi : IAppAwareBackofficeApi
  {
    /// <summary>
    /// Get all user roles
    /// </summary>
    Task<IList<IUserRole>> GetAll();

    /// <summary>
    /// Get role by id
    /// </summary>
    Task<IUserRole> GetById(string id);

    /// <summary>
    /// Create or update a role
    /// </summary>
    Task<EntityResult<IUserRole>> Save(IUserRole model);

    /// <summary>
    /// Deletes a role
    /// </summary>
    Task<EntityResult<IUserRole>> Delete(string id);
  }
}
