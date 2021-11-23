using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace zero.Identity;

public class UserService : BackofficeApi, IUserService
{
  protected UserManager<ZeroUser> UserManager { get; private set; }

  public UserService(IStoreContext store, UserManager<ZeroUser> userManager) : base(store, isCoreDatabase: true)
  {
    UserManager = userManager;
  }


  /// <inheritdoc />
  public async Task<ZeroUser> GetUserById(string id)
  {
    ZeroUser user = await UserManager.FindByIdAsync(id);
    return user;
  }


  /// <inheritdoc />
  public async Task<ZeroUser> GetUserByEmail(string email)
  {
    ZeroUser user = await UserManager.FindByEmailAsync(email);
    return user;
  }


  /// <inheritdoc />
  public async Task<Dictionary<string, ZeroUser>> GetByIds(params string[] ids)
  {
    return await GetByIds<ZeroUser>(ids);
  }


  /// <inheritdoc />
  public async Task<IList<ZeroUser>> GetAll()
  {
    return await Session.Query<ZeroUser>()
      .OrderByDescending(x => x.CreatedDate)
      .ToListAsync();
  }


  /// <inheritdoc />
  public async Task<Paged<ZeroUser>> GetByQuery(ListQuery<ZeroUser> query)
  {
    string currentUserId = UserManager.GetUserId(Context.Context.BackofficeUser);

    query.SearchSelector = user => user.Name;

    return await Session.Query<ZeroUser>()
      .ToQueriedListAsync(query);
  }


  /// <inheritdoc />
  public async Task<EntityResult<ZeroUser>> Save(ZeroUser model)
  {
    bool updateSecurityStamp = false;

    if (!model.Id.IsNullOrEmpty())
    {
      ZeroUser origin = await GetUserById(model.Id);
      updateSecurityStamp = origin != null && model.PasswordHash != origin.PasswordHash;
    }

    EntityResult<ZeroUser> result = await SaveModel(model); //, new UserValidator<User>());

    if (updateSecurityStamp)
    {
      await UserManager.UpdateSecurityStampAsync(model);
    }

    return result;
  }


  /// <inheritdoc />
  public async Task<EntityResult<ZeroUser>> Delete(string id)
  {
    return await DeleteById<ZeroUser>(id);
  }


  /// <inheritdoc />
  public async Task<EntityResult<string>> HashPassword(ZeroUser user, string currentPassword, string newPassword, string confirmNewPassword)
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
  public async Task<EntityResult<ZeroUser>> UpdatePassword(ZeroUser user, string currentPassword, string newPassword)
  {
    if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
    {
      return EntityResult<ZeroUser>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
    }

    if (user == null)
    {
      return EntityResult<ZeroUser>.Fail("@errors.changepassword.nouser");
    }

    IdentityResult identityResult = await UserManager.ChangePasswordAsync(user as ZeroUser, currentPassword, newPassword);

    if (!identityResult.Succeeded)
    {
      EntityResult<ZeroUser> result = EntityResult<ZeroUser>.Fail();
        
      foreach (IdentityError error in identityResult.Errors)
      {
        result.AddError(error.Description);
      }

      return result;
    }

    return EntityResult<ZeroUser>.Success(user);
  }


  /// <inheritdoc />
  public async Task<EntityResult<ZeroUser>> Enable(ZeroUser user)
  {
    return await UpdateActiveState(user, true);
  }


  /// <inheritdoc />
  public async Task<EntityResult<ZeroUser>> Disable(ZeroUser user)
  {
    return await UpdateActiveState(user, false);
  }


  /// <summary>
  /// Updates the active state of user.
  /// If IsActive=false, the user cannot login anymore
  /// </summary>
  async Task<EntityResult<ZeroUser>> UpdateActiveState(ZeroUser user, bool isActive)
  {
    user.IsActive = isActive;

    IdentityResult identityResult = await UserManager.UpdateAsync(user as ZeroUser);

    if (!identityResult.Succeeded)
    {
      EntityResult<ZeroUser> result = EntityResult<ZeroUser>.Fail();

      foreach (IdentityError error in identityResult.Errors)
      {
        result.AddError(error.Description);
      }

      return result;
    }

    await UserManager.UpdateSecurityStampAsync(user as ZeroUser);

    return EntityResult<ZeroUser>.Success(user);
  }
}


public interface IUserService : IBackofficeApi
{
  /// <summary>
  /// Find user by id
  /// </summary>
  Task<ZeroUser> GetUserById(string id);

  /// <summary>
  /// Find user by email
  /// </summary>
  Task<ZeroUser> GetUserByEmail(string email);

  /// <summary>
  /// Get users by ids
  /// </summary>
  Task<Dictionary<string, ZeroUser>> GetByIds(params string[] ids);

  /// <summary>
  /// Get all users for the selected application
  /// </summary>
  Task<IList<ZeroUser>> GetAll();

  /// <summary>
  /// Get all available users (with query)
  /// </summary>
  Task<Paged<ZeroUser>> GetByQuery(ListQuery<ZeroUser> query);

  /// <summary>
  /// Creates or updates a user
  /// </summary>
  Task<EntityResult<ZeroUser>> Save(ZeroUser model);

  /// <summary>
  /// Deletes a user
  /// </summary>
  Task<EntityResult<ZeroUser>> Delete(string id);

  /// <summary>
  /// Changes the password of the current user.
  /// User is logged out if this operation succeeds.
  /// </summary>
  Task<EntityResult<ZeroUser>> UpdatePassword(ZeroUser user, string currentPassword, string newPassword);

  /// <summary>
  /// Tries to hash a new password
  /// </summary>
  Task<EntityResult<string>> HashPassword(ZeroUser user, string currentPassword, string newPassword, string confirmNewPassword);

  /// <summary>
  /// Enables a user
  /// </summary>
  Task<EntityResult<ZeroUser>> Enable(ZeroUser user);

  /// <summary>
  /// Disables a user
  /// </summary>
  Task<EntityResult<ZeroUser>> Disable(ZeroUser user);
}