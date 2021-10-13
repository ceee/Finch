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
using zero.Core.Collections;
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


    public UserRolesApi(IHttpContextAccessor httpContextAccessor, UserManager<BackofficeUser> userManager, RoleManager<BackofficeUserRole> roleManager, ICollectionContext store) : base(store, isCoreDatabase: true)
    {
      HttpContextAccessor = httpContextAccessor;
      UserManager = userManager;
      RoleManager = roleManager;
    }


    /// <inheritdoc />
    public async Task<IList<BackofficeUserRole>> GetAll()
    {
      return await Session.Query<BackofficeUserRole>().OrderBy(x => x.Sort).ThenBy(x => x.Name).ToListAsync();
    }


    /// <inheritdoc />
    public async Task<BackofficeUserRole> GetById(string id)
    {
      return await RoleManager.FindByIdAsync(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<BackofficeUserRole>> Save(BackofficeUserRole model)
    {
      ValidationResult validation = await new UserRoleValidator().ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityResult<BackofficeUserRole>.Fail(validation);
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.Alias = Safenames.Alias(model.Name);

      await Session.StoreAsync(model);

      string id = Session.Advanced.GetDocumentId(model);

      await Session.SaveChangesAsync();

      if (model.Id.IsNullOrEmpty())
      {
        model.Id = id;
      }

      return EntityResult<BackofficeUserRole>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<BackofficeUserRole>> Delete(string id)
    {
      BackofficeUserRole country = await Session.LoadAsync<BackofficeUserRole>(id);

      if (country == null)
      {
        return EntityResult<BackofficeUserRole>.Fail("@errors.ondelete.idnotfound");
      }

      Session.Delete(country);

      await Session.SaveChangesAsync();

      return EntityResult<BackofficeUserRole>.Success();
    }
  }


  public interface IUserRolesApi : IBackofficeApi
  {
    /// <summary>
    /// Get all user roles
    /// </summary>
    Task<IList<BackofficeUserRole>> GetAll();

    /// <summary>
    /// Get role by id
    /// </summary>
    Task<BackofficeUserRole> GetById(string id);

    /// <summary>
    /// Create or update a role
    /// </summary>
    Task<EntityResult<BackofficeUserRole>> Save(BackofficeUserRole model);

    /// <summary>
    /// Deletes a role
    /// </summary>
    Task<EntityResult<BackofficeUserRole>> Delete(string id);
  }
}
