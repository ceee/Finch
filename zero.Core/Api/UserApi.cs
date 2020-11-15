using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Api
{
  public class UserApi : BackofficeApi, IUserApi
  {
    protected UserManager<BackofficeUser> UserManager { get; private set; }

    protected IZeroContext Context { get; set; }

    public UserApi(IBackofficeStore store, UserManager<BackofficeUser> userManager, IZeroContext context) : base(store)
    {
      UserManager = userManager;
      Context = context;
    }


    /// <inheritdoc />
    public async Task<IBackofficeUser> GetUserById(string id)
    {
      IBackofficeUser user = await UserManager.FindByIdAsync(id);
      return user;
    }


    /// <inheritdoc />
    public async Task<IBackofficeUser> GetUserByEmail(string email)
    {
      IBackofficeUser user = await UserManager.FindByEmailAsync(email);
      return user;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, IBackofficeUser>> GetByIds(params string[] ids)
    {
      return await GetByIds<IBackofficeUser>(ids);
    }


    /// <inheritdoc />
    public async Task<IList<IBackofficeUser>> GetAll()
    {
      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await session.Query<IBackofficeUser>()
        .OrderByDescending(x => x.CreatedDate)
        .ToListAsync();
    }


    /// <inheritdoc />
    public async Task<ListResult<IBackofficeUser>> GetByQuery(ListQuery<IBackofficeUser> query)
    {
      string currentUserId = UserManager.GetUserId(Context.BackofficeUser);

      query.SearchSelector = user => user.Name;

      using IAsyncDocumentSession session = Store.OpenAsyncSession();
      return await session.Query<IBackofficeUser>()
        .ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IBackofficeUser>> Save(IBackofficeUser model)
    {
      return await SaveModel(model); //, new UserValidator<User>());
    }


    /// <inheritdoc />
    public async Task<EntityResult<IBackofficeUser>> Delete(string id)
    {
      return await DeleteById<IBackofficeUser>(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IBackofficeUser>> UpdatePassword(IBackofficeUser user, string currentPassword, string newPassword)
    {
      if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
      {
        return EntityResult<IBackofficeUser>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
      }

      if (user == null)
      {
        return EntityResult<IBackofficeUser>.Fail("@errors.changepassword.nouser");
      }

      IdentityResult identityResult = await UserManager.ChangePasswordAsync(user as BackofficeUser, currentPassword, newPassword);

      if (!identityResult.Succeeded)
      {
        EntityResult<IBackofficeUser> result = EntityResult<IBackofficeUser>.Fail();
        
        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      return EntityResult<IBackofficeUser>.Success(user);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IBackofficeUser>> Enable(IBackofficeUser user)
    {
      return await UpdateActiveState(user, true);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IBackofficeUser>> Disable(IBackofficeUser user)
    {
      return await UpdateActiveState(user, false);
    }


    /// <summary>
    /// Updates the active state of user.
    /// If IsActive=false, the user cannot login anymore
    /// </summary>
    async Task<EntityResult<IBackofficeUser>> UpdateActiveState(IBackofficeUser user, bool isActive)
    {
      user.IsActive = isActive;

      IdentityResult identityResult = await UserManager.UpdateAsync(user as BackofficeUser);

      if (!identityResult.Succeeded)
      {
        EntityResult<IBackofficeUser> result = EntityResult<IBackofficeUser>.Fail();

        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      await UserManager.UpdateSecurityStampAsync(user as BackofficeUser);

      return EntityResult<IBackofficeUser>.Success(user);
    }
  }


  public interface IUserApi : IBackofficeApi
  {
    /// <summary>
    /// Find user by id
    /// </summary>
    Task<IBackofficeUser> GetUserById(string id);

    /// <summary>
    /// Find user by email
    /// </summary>
    Task<IBackofficeUser> GetUserByEmail(string email);

    /// <summary>
    /// Get users by ids
    /// </summary>
    Task<Dictionary<string, IBackofficeUser>> GetByIds(params string[] ids);

    /// <summary>
    /// Get all users for the selected application
    /// </summary>
    Task<IList<IBackofficeUser>> GetAll();

    /// <summary>
    /// Get all available users (with query)
    /// </summary>
    Task<ListResult<IBackofficeUser>> GetByQuery(ListQuery<IBackofficeUser> query);

    /// <summary>
    /// Creates or updates a user
    /// </summary>
    Task<EntityResult<IBackofficeUser>> Save(IBackofficeUser model);

    /// <summary>
    /// Deletes a user
    /// </summary>
    Task<EntityResult<IBackofficeUser>> Delete(string id);

    /// <summary>
    /// Changes the password of the current user.
    /// User is logged out if this operation succeeds.
    /// </summary>
    Task<EntityResult<IBackofficeUser>> UpdatePassword(IBackofficeUser user, string currentPassword, string newPassword);

    /// <summary>
    /// Enables a user
    /// </summary>
    Task<EntityResult<IBackofficeUser>> Enable(IBackofficeUser user);

    /// <summary>
    /// Disables a user
    /// </summary>
    Task<EntityResult<IBackofficeUser>> Disable(IBackofficeUser user);
  }
}
