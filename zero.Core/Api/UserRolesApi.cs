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
  public class UserRolesApi : IUserRolesApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected UserManager<User> UserManager { get; private set; }

    protected RoleManager<UserRole> RoleManager { get; private set; }

    private ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;


    public UserRolesApi(IDocumentStore raven, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, RoleManager<UserRole> roleManager)
    {
      Raven = raven;
      HttpContextAccessor = httpContextAccessor;
      UserManager = userManager;
      RoleManager = roleManager;
    }


    /// <inheritdoc />
    public async Task<IList<UserRole>> GetAll()
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<UserRole>().OrderBy(x => x.Sort).ThenBy(x => x.Name).ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<UserRole> GetById(string id)
    {
      return await RoleManager.FindByIdAsync(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<UserRole>> Save(UserRole model)
    {
      ValidationResult validation = await new UserRoleValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<UserRole>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.AppId = "zero.applications.1-A"; // TODO real app id
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.Alias = Alias.Generate(model.Name);

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

      return EntityResult<UserRole>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<UserRole>> Delete(string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        UserRole country = await session.LoadAsync<UserRole>(id);

        if (country == null)
        {
          return EntityResult<UserRole>.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(country);

        await session.SaveChangesAsync();
      }

      return EntityResult<UserRole>.Success();
    }
  }


  public interface IUserRolesApi
  {
    /// <summary>
    /// Get all user roles
    /// </summary>
    Task<IList<UserRole>> GetAll();

    /// <summary>
    /// Get role by id
    /// </summary>
    Task<UserRole> GetById(string id);

    /// <summary>
    /// Create or update a role
    /// </summary>
    Task<EntityResult<UserRole>> Save(UserRole model);

    /// <summary>
    /// Deletes a role
    /// </summary>
    Task<EntityResult<UserRole>> Delete(string id);
  }
}
