using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class UserApi<T> : AppAwareBackofficeApi, IUserApi<T> where T : class, IUser
  {
    protected UserManager<T> UserManager { get; private set; }

    public UserApi(IBackofficeStore store, UserManager<T> userManager) : base(store)
    {
      UserManager = userManager;
    }


    /// <inheritdoc />
    public async Task<T> GetUserById(string id)
    {
      T user = await UserManager.FindByIdAsync(id);
      return user;
    }


    /// <inheritdoc />
    public async Task<T> GetUserByEmail(string email)
    {
      T user = await UserManager.FindByEmailAsync(email);
      return user;
    }


    /// <inheritdoc />
    public async Task<IList<T>> GetAll(string appId = null)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .Scope(appId)
          .OrderByDescending(x => x.CreatedDate)
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetByQuery(ListQuery<T> query, string appId = null)
    {
      query.SearchSelector = user => user.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .Scope(appId)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save(T model)
    {
      return await SaveModel(model); //, new UserValidator<User>());
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Delete(string id)
    {
      return await DeleteById<T>(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> UpdatePassword(T user, string currentPassword, string newPassword)
    {
      if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
      {
        return EntityResult<T>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
      }

      if (user == null)
      {
        return EntityResult<T>.Fail("@errors.changepassword.nouser");
      }

      IdentityResult identityResult = await UserManager.ChangePasswordAsync(user, currentPassword, newPassword);

      if (!identityResult.Succeeded)
      {
        EntityResult<T> result = EntityResult<T>.Fail();
        
        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      return EntityResult<T>.Success(user);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Enable(T user)
    {
      return await UpdateActiveState(user, true);
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Disable(T user)
    {
      return await UpdateActiveState(user, false);
    }


    /// <summary>
    /// Updates the active state of user.
    /// If IsActive=false, the user cannot login anymore
    /// </summary>
    async Task<EntityResult<T>> UpdateActiveState(T user, bool isActive)
    {
      user.IsActive = isActive;

      IdentityResult identityResult = await UserManager.UpdateAsync(user);

      if (!identityResult.Succeeded)
      {
        EntityResult<T> result = EntityResult<T>.Fail();

        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      await UserManager.UpdateSecurityStampAsync(user);

      return EntityResult<T>.Success(user);
    }
  }


  public interface IUserApi<T> : IAppAwareBackofficeApi where T : class, IUser
  {
    /// <summary>
    /// Find user by id
    /// </summary>
    Task<T> GetUserById(string id);

    /// <summary>
    /// Find user by email
    /// </summary>
    Task<T> GetUserByEmail(string email);  

    /// <summary>
    /// Get all users for the selected application
    /// </summary>
    Task<IList<T>> GetAll(string appId = null);

    /// <summary>
    /// Get all available users (with query)
    /// </summary>
    Task<ListResult<T>> GetByQuery(ListQuery<T> query, string appId = null);

    /// <summary>
    /// Creates or updates a user
    /// </summary>
    Task<EntityResult<T>> Save(T model);

    /// <summary>
    /// Deletes a user
    /// </summary>
    Task<EntityResult<T>> Delete(string id);

    /// <summary>
    /// Changes the password of the current user.
    /// User is logged out if this operation succeeds.
    /// </summary>
    Task<EntityResult<T>> UpdatePassword(T user, string currentPassword, string newPassword);

    /// <summary>
    /// Enables a user
    /// </summary>
    Task<EntityResult<T>> Enable(T user);

    /// <summary>
    /// Disables a user
    /// </summary>
    Task<EntityResult<T>> Disable(T user);
  }
}
