using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Identity;

namespace zero.Core.Api
{
  public class UserApi : AppAwareBackofficeApi, IUserApi
  {
    protected UserManager<User> UserManager { get; private set; }

    public UserApi(IBackofficeStore store, UserManager<User> userManager) : base(store)
    {
      UserManager = userManager;
    }


    /// <inheritdoc />
    public async Task<User> GetUserById(string id)
    {
      User user = await UserManager.FindByIdAsync(id);
      return user;
    }


    /// <inheritdoc />
    public async Task<User> GetUserByEmail(string email)
    {
      User user = await UserManager.FindByEmailAsync(email);
      return user;
    }


    /// <inheritdoc />
    public async Task<IList<User>> GetAll(string appId = null)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<User>()
          .Scope(appId)
          .OrderByDescending(x => x.CreatedDate)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<User>> GetByQuery(ListQuery<User> query, string appId = null)
    {
      query.SearchSelector = user => user.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<User>()
          .Scope(appId)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<User>> Save(User model)
    {
      return await Save(model); //, new UserValidator<User>());
    }


    /// <inheritdoc />
    public async Task<EntityResult<User>> Delete(string id)
    {
      return await DeleteById<User>(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<User>> UpdatePassword(User user, string currentPassword, string newPassword)
    {
      if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
      {
        return EntityResult<User>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
      }

      if (user == null)
      {
        return EntityResult<User>.Fail("@errors.changepassword.nouser");
      }

      IdentityResult identityResult = await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);

      if (!identityResult.Succeeded)
      {
        EntityResult<User> result = EntityResult<User>.Fail();
        
        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      return EntityResult<User>.Success(user);
    }


    /// <inheritdoc />
    public async Task<EntityResult<User>> Enable(User user)
    {
      return await UpdateActiveState(user, true);
    }


    /// <inheritdoc />
    public async Task<EntityResult<User>> Disable(User user)
    {
      return await UpdateActiveState(user, false);
    }


    /// <summary>
    /// Updates the active state of user.
    /// If IsActive=false, the user cannot login anymore
    /// </summary>
    async Task<EntityResult<User>> UpdateActiveState(User user, bool isActive)
    {
      user.IsActive = isActive;

      IdentityResult identityResult = await UserManager.UpdateAsync(user);

      if (!identityResult.Succeeded)
      {
        EntityResult<User> result = EntityResult<User>.Fail();

        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      await UserManager.UpdateSecurityStampAsync(user);

      return EntityResult<User>.Success(user);
    }
  }


  public interface IUserApi : IAppAwareBackofficeApi
  {
    /// <summary>
    /// Find user by id
    /// </summary>
    Task<User> GetUserById(string id);

    /// <summary>
    /// Find user by email
    /// </summary>
    Task<User> GetUserByEmail(string email);  

    /// <summary>
    /// Get all users for the selected application
    /// </summary>
    Task<IList<User>> GetAll(string appId = null);

    /// <summary>
    /// Get all available users (with query)
    /// </summary>
    Task<ListResult<User>> GetByQuery(ListQuery<User> query, string appId = null);

    /// <summary>
    /// Creates or updates a user
    /// </summary>
    Task<EntityResult<User>> Save(User model);

    /// <summary>
    /// Deletes a user
    /// </summary>
    Task<EntityResult<User>> Delete(string id);

    /// <summary>
    /// Changes the password of the current user.
    /// User is logged out if this operation succeeds.
    /// </summary>
    Task<EntityResult<User>> UpdatePassword(User user, string currentPassword, string newPassword);

    /// <summary>
    /// Enables a user
    /// </summary>
    Task<EntityResult<User>> Enable(User user);

    /// <summary>
    /// Disables a user
    /// </summary>
    Task<EntityResult<User>> Disable(User user);
  }
}
