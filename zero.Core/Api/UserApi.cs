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

    public UserApi(IBackofficeStore store, UserManager<BackofficeUser> userManager, IZeroContext context) : base(store, isCoreDatabase: true)
    {
      UserManager = userManager;
      Context = context;
    }


    /// <inheritdoc />
    public async Task<BackofficeUser> GetUserById(string id)
    {
      BackofficeUser user = await UserManager.FindByIdAsync(id);
      return user;
    }


    /// <inheritdoc />
    public async Task<BackofficeUser> GetUserByEmail(string email)
    {
      BackofficeUser user = await UserManager.FindByEmailAsync(email);
      return user;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, BackofficeUser>> GetByIds(params string[] ids)
    {
      return await GetByIds<BackofficeUser>(ids);
    }


    /// <inheritdoc />
    public async Task<IList<BackofficeUser>> GetAll()
    {
      using IAsyncDocumentSession session = Session();
      return await session.Query<BackofficeUser>()
        .OrderByDescending(x => x.CreatedDate)
        .ToListAsync();
    }


    /// <inheritdoc />
    public async Task<ListResult<BackofficeUser>> GetByQuery(ListQuery<BackofficeUser> query)
    {
      string currentUserId = UserManager.GetUserId(Context.BackofficeUser);

      query.SearchSelector = user => user.Name;

      using IAsyncDocumentSession session = Session();
      return await session.Query<BackofficeUser>()
        .ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<BackofficeUser>> Save(BackofficeUser model)
    {
      bool updateSecurityStamp = false;

      if (!model.Id.IsNullOrEmpty())
      {
        BackofficeUser origin = await GetUserById(model.Id);
        updateSecurityStamp = origin != null && model.PasswordHash != origin.PasswordHash;
      }

      EntityResult<BackofficeUser> result = await SaveModel(model); //, new UserValidator<User>());

      if (updateSecurityStamp)
      {
        await UserManager.UpdateSecurityStampAsync(model);
      }

      return result;
    }


    /// <inheritdoc />
    public async Task<EntityResult<BackofficeUser>> Delete(string id)
    {
      return await DeleteById<BackofficeUser>(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<string>> HashPassword(BackofficeUser user, string currentPassword, string newPassword, string confirmNewPassword)
    {
      if (newPassword != confirmNewPassword)
      {
        return EntityResult<string>.Fail(nameof(newPassword), "@errors.changepassword.newpasswordsnotmatching");
      }

      if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
      {
        return EntityResult<string>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
      }

      if (user == null)
      {
        return EntityResult<string>.Fail("@errors.changepassword.nouser");
      }

      if (UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword) != PasswordVerificationResult.Success)
      {
        return EntityResult<string>.Fail("@errors.changepassword.passwordincorrect");
      }

      // validate new password
      List<IdentityError> errors = new();
      bool isValid = true;
      foreach (var v in UserManager.PasswordValidators)
      {
        var result = await v.ValidateAsync(UserManager, user, newPassword);
        if (!result.Succeeded)
        {
          if (result.Errors.Any())
          {
            errors.AddRange(result.Errors);
          }

          isValid = false;
        }
      }

      if (!isValid)
      {
        EntityResult<string> result = EntityResult<string>.Fail();
        foreach (IdentityError error in errors)
        {
          result.AddError(error.Description);
        }
        return result;
      }

      return EntityResult<string>.Success(UserManager.PasswordHasher.HashPassword(user, newPassword));
    }


    /// <inheritdoc />
    public async Task<EntityResult<BackofficeUser>> UpdatePassword(BackofficeUser user, string currentPassword, string newPassword)
    {
      if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
      {
        return EntityResult<BackofficeUser>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
      }

      if (user == null)
      {
        return EntityResult<BackofficeUser>.Fail("@errors.changepassword.nouser");
      }

      IdentityResult identityResult = await UserManager.ChangePasswordAsync(user as BackofficeUser, currentPassword, newPassword);

      if (!identityResult.Succeeded)
      {
        EntityResult<BackofficeUser> result = EntityResult<BackofficeUser>.Fail();
        
        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      return EntityResult<BackofficeUser>.Success(user);
    }


    /// <inheritdoc />
    public async Task<EntityResult<BackofficeUser>> Enable(BackofficeUser user)
    {
      return await UpdateActiveState(user, true);
    }


    /// <inheritdoc />
    public async Task<EntityResult<BackofficeUser>> Disable(BackofficeUser user)
    {
      return await UpdateActiveState(user, false);
    }


    /// <summary>
    /// Updates the active state of user.
    /// If IsActive=false, the user cannot login anymore
    /// </summary>
    async Task<EntityResult<BackofficeUser>> UpdateActiveState(BackofficeUser user, bool isActive)
    {
      user.IsActive = isActive;

      IdentityResult identityResult = await UserManager.UpdateAsync(user as BackofficeUser);

      if (!identityResult.Succeeded)
      {
        EntityResult<BackofficeUser> result = EntityResult<BackofficeUser>.Fail();

        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      await UserManager.UpdateSecurityStampAsync(user as BackofficeUser);

      return EntityResult<BackofficeUser>.Success(user);
    }
  }


  public interface IUserApi : IBackofficeApi
  {
    /// <summary>
    /// Find user by id
    /// </summary>
    Task<BackofficeUser> GetUserById(string id);

    /// <summary>
    /// Find user by email
    /// </summary>
    Task<BackofficeUser> GetUserByEmail(string email);

    /// <summary>
    /// Get users by ids
    /// </summary>
    Task<Dictionary<string, BackofficeUser>> GetByIds(params string[] ids);

    /// <summary>
    /// Get all users for the selected application
    /// </summary>
    Task<IList<BackofficeUser>> GetAll();

    /// <summary>
    /// Get all available users (with query)
    /// </summary>
    Task<ListResult<BackofficeUser>> GetByQuery(ListQuery<BackofficeUser> query);

    /// <summary>
    /// Creates or updates a user
    /// </summary>
    Task<EntityResult<BackofficeUser>> Save(BackofficeUser model);

    /// <summary>
    /// Deletes a user
    /// </summary>
    Task<EntityResult<BackofficeUser>> Delete(string id);

    /// <summary>
    /// Changes the password of the current user.
    /// User is logged out if this operation succeeds.
    /// </summary>
    Task<EntityResult<BackofficeUser>> UpdatePassword(BackofficeUser user, string currentPassword, string newPassword);

    /// <summary>
    /// Tries to hash a new password
    /// </summary>
    Task<EntityResult<string>> HashPassword(BackofficeUser user, string currentPassword, string newPassword, string confirmNewPassword);

    /// <summary>
    /// Enables a user
    /// </summary>
    Task<EntityResult<BackofficeUser>> Enable(BackofficeUser user);

    /// <summary>
    /// Disables a user
    /// </summary>
    Task<EntityResult<BackofficeUser>> Disable(BackofficeUser user);
  }
}
