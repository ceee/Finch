using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;

namespace zero.Identity;

public class UserService : IUserService
{
  protected UserManager<ZeroUser> UserManager { get; private set; }

  protected IStoreOperations Operations { get; private set; }

  protected IZeroContext Context { get; private set; }

  public UserService(ISharedStoreOperationsWithInactive operations, IZeroContext context, IZeroOptions options, UserManager<ZeroUser> userManager)
  {
    UserManager = userManager;
    Operations = operations;
    Context = context;
  }


  /// <inheritdoc />
  public async Task<ZeroUser> GetCurrentUser()
  {
    return await UserManager.GetUserAsync(Context.BackofficeUser);
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
    return await Operations.Load<ZeroUser>(ids);
  }


  /// <inheritdoc />
  public async Task<Paged<ZeroUser>> GetAll(int pageNumber, int pageSize)
  {
    return await Operations.Load<ZeroUser>(pageNumber, pageSize, q => q.OrderByDescending(x => x.CreatedDate));
  }


  /// <inheritdoc />
  public async Task<Result<ZeroUser>> Save(ZeroUser model)
  {
    bool updateSecurityStamp = false;
    bool isUpdate = false;

    if (!model.Id.IsNullOrEmpty())
    {
      // TODO throws "Attempted to associate a different object with id ..."
      //ZeroUser origin = await GetUserById(model.Id);
      isUpdate = true; // origin != null;
      updateSecurityStamp = true; // origin != null && model.PasswordHash != origin.PasswordHash;
    }

    Result<ZeroUser> result = isUpdate ? await Operations.Update(model) : await Operations.Create(model);

    if (updateSecurityStamp)
    {
      await UserManager.UpdateSecurityStampAsync(model);
    }

    return result;
  }


  /// <inheritdoc />
  public async Task<Result<ZeroUser>> Delete(string id)
  {
    return await Operations.Delete<ZeroUser>(id);
  }


  /// <inheritdoc />
  public async Task<Result<string>> HashPassword(ZeroUser user, string currentPassword, string newPassword, string confirmNewPassword)
  {
    if (newPassword != confirmNewPassword)
    {
      return Result<string>.Fail(nameof(newPassword), "@errors.changepassword.newpasswordsnotmatching");
    }

    if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
    {
      return Result<string>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
    }

    if (user == null)
    {
      return Result<string>.Fail("@errors.changepassword.nouser");
    }

    if (UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword) != PasswordVerificationResult.Success)
    {
      return Result<string>.Fail("@errors.changepassword.passwordincorrect");
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
      Result<string> result = Result<string>.Fail();
      foreach (IdentityError error in errors)
      {
        result.AddError(error.Description);
      }
      return result;
    }

    return Result<string>.Success(UserManager.PasswordHasher.HashPassword(user, newPassword));
  }


  /// <inheritdoc />
  public async Task<Result<ZeroUser>> UpdatePassword(ZeroUser user, string currentPassword, string newPassword)
  {
    if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
    {
      return Result<ZeroUser>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
    }

    if (user == null)
    {
      return Result<ZeroUser>.Fail("@errors.changepassword.nouser");
    }

    IdentityResult identityResult = await UserManager.ChangePasswordAsync(user as ZeroUser, currentPassword, newPassword);

    if (!identityResult.Succeeded)
    {
      Result<ZeroUser> result = Result<ZeroUser>.Fail();
        
      foreach (IdentityError error in identityResult.Errors)
      {
        result.AddError(error.Description);
      }

      return result;
    }

    return Result<ZeroUser>.Success(user);
  }


  /// <inheritdoc />
  public async Task<Result<ZeroUser>> Enable(ZeroUser user)
  {
    return await UpdateActiveState(user, true);
  }


  /// <inheritdoc />
  public async Task<Result<ZeroUser>> Disable(ZeroUser user)
  {
    return await UpdateActiveState(user, false);
  }


  /// <summary>
  /// Updates the active state of user.
  /// If IsActive=false, the user cannot login anymore
  /// </summary>
  async Task<Result<ZeroUser>> UpdateActiveState(ZeroUser user, bool isActive)
  {
    user.IsActive = isActive;

    IdentityResult identityResult = await UserManager.UpdateAsync(user as ZeroUser);

    if (!identityResult.Succeeded)
    {
      Result<ZeroUser> result = Result<ZeroUser>.Fail();

      foreach (IdentityError error in identityResult.Errors)
      {
        result.AddError(error.Description);
      }

      return result;
    }

    await UserManager.UpdateSecurityStampAsync(user as ZeroUser);

    return Result<ZeroUser>.Success(user);
  }


  /// <inheritdoc />
  public async Task<bool> TrySwitchApp(string appId)
  {
    IZeroDocumentSession session = Context.Store.Session(global: true);
    ZeroUser user = await UserManager.GetUserAsync(Context.BackofficeUser);

    if (user == null || appId.IsNullOrEmpty())
    {
      return false;
    }

    string[] allowedAppIds = user.GetAllowedAppIds();

    bool isMainId = appId.Equals(user.AppId, StringComparison.InvariantCultureIgnoreCase);
    bool isAllowedId = allowedAppIds.Contains(appId, StringComparer.InvariantCultureIgnoreCase);

    if (user.IsSuper || isMainId || isAllowedId)
    {
      user.CurrentAppId = appId;

      //byte[] bytes = new byte[20];
      //RandomNumberGenerator.Fill(bytes);
      //user.SecurityStamp = Base32.ToBase32(bytes); // TODO update security stamp but Base32 is .net core internal

      await session.StoreAsync(user);
      await session.SaveChangesAsync();

      return true;
    }

    return false;
  }
}


public interface IUserService
{
  /// <summary>
  /// Get currently logged-in backoffice user
  /// </summary>
  Task<ZeroUser> GetCurrentUser();

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
  /// Get all users for the current application
  /// </summary>
  Task<Paged<ZeroUser>> GetAll(int pageNumber, int pageSize);

  /// <summary>
  /// Creates or updates a user
  /// </summary>
  Task<Result<ZeroUser>> Save(ZeroUser model);

  /// <summary>
  /// Deletes a user
  /// </summary>
  Task<Result<ZeroUser>> Delete(string id);

  /// <summary>
  /// Changes the password of the current user.
  /// User is logged out if this operation succeeds.
  /// </summary>
  Task<Result<ZeroUser>> UpdatePassword(ZeroUser user, string currentPassword, string newPassword);

  /// <summary>
  /// Tries to hash a new password
  /// </summary>
  Task<Result<string>> HashPassword(ZeroUser user, string currentPassword, string newPassword, string confirmNewPassword);

  /// <summary>
  /// Enables a user
  /// </summary>
  Task<Result<ZeroUser>> Enable(ZeroUser user);

  /// <summary>
  /// Disables a user
  /// </summary>
  Task<Result<ZeroUser>> Disable(ZeroUser user);

  /// <summary>
  /// Tries to switch the currently loaded backoffice application for the current user
  /// </summary>
  Task<bool> TrySwitchApp(string appId);
}