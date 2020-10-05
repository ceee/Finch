using Microsoft.AspNetCore.Http;
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
  public class UserApi : AppAwareBackofficeApi, IUserApi
  {
    protected UserManager<User> UserManager { get; private set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    public UserApi(IBackofficeStore store, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) : base(store)
    {
      UserManager = userManager;
      HttpContextAccessor = httpContextAccessor;
    }


    /// <inheritdoc />
    public async Task<IUser> GetUserById(string id)
    {
      IUser user = await UserManager.FindByIdAsync(id);
      return user;
    }


    /// <inheritdoc />
    public async Task<IUser> GetUserByEmail(string email)
    {
      IUser user = await UserManager.FindByEmailAsync(email);
      return user;
    }


    /// <inheritdoc />
    public async Task<Dictionary<string, IUser>> GetByIds(params string[] ids)
    {
      return await GetByIds<IUser>(ids);
    }


    /// <inheritdoc />
    public async Task<IList<IUser>> GetAll()
    {
      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await session.Query<IUser>()
        .Scope(Scope)
        .OrderByDescending(x => x.CreatedDate)
        .ToListAsync();
    }


    /// <inheritdoc />
    public async Task<ListResult<IUser>> GetByQuery(ListQuery<IUser> query)
    {
      string currentUserId = (await UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User))?.Id;
      HashSet<string> appIds = new HashSet<string>() { Constants.Database.SharedAppId, Scope.AppId };

      query.SearchSelector = user => user.Name;

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      return await session.Query<IUser>()
        .Where(x => x.AppId.In(appIds) || x.Id == currentUserId)
        .ToQueriedListAsync(query);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IUser>> Save(IUser model)
    {
      return await SaveModel(model); //, new UserValidator<User>());
    }


    /// <inheritdoc />
    public async Task<EntityResult<IUser>> Delete(string id)
    {
      return await DeleteById<IUser>(id);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IUser>> UpdatePassword(IUser user, string currentPassword, string newPassword)
    {
      if (currentPassword.IsNullOrWhiteSpace() || newPassword.IsNullOrWhiteSpace())
      {
        return EntityResult<IUser>.Fail(nameof(currentPassword), "@errors.changepassword.emptyfields");
      }

      if (user == null)
      {
        return EntityResult<IUser>.Fail("@errors.changepassword.nouser");
      }

      IdentityResult identityResult = await UserManager.ChangePasswordAsync(user as User, currentPassword, newPassword);

      if (!identityResult.Succeeded)
      {
        EntityResult<IUser> result = EntityResult<IUser>.Fail();
        
        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      return EntityResult<IUser>.Success(user);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IUser>> Enable(IUser user)
    {
      return await UpdateActiveState(user, true);
    }


    /// <inheritdoc />
    public async Task<EntityResult<IUser>> Disable(IUser user)
    {
      return await UpdateActiveState(user, false);
    }


    /// <summary>
    /// Updates the active state of user.
    /// If IsActive=false, the user cannot login anymore
    /// </summary>
    async Task<EntityResult<IUser>> UpdateActiveState(IUser user, bool isActive)
    {
      user.IsActive = isActive;

      IdentityResult identityResult = await UserManager.UpdateAsync(user as User);

      if (!identityResult.Succeeded)
      {
        EntityResult<IUser> result = EntityResult<IUser>.Fail();

        foreach (IdentityError error in identityResult.Errors)
        {
          result.AddError(error.Description);
        }

        return result;
      }

      await UserManager.UpdateSecurityStampAsync(user as User);

      return EntityResult<IUser>.Success(user);
    }
  }


  public interface IUserApi : IAppAwareBackofficeApi
  {
    /// <summary>
    /// Find user by id
    /// </summary>
    Task<IUser> GetUserById(string id);

    /// <summary>
    /// Find user by email
    /// </summary>
    Task<IUser> GetUserByEmail(string email);

    /// <summary>
    /// Get users by ids
    /// </summary>
    Task<Dictionary<string, IUser>> GetByIds(params string[] ids);

    /// <summary>
    /// Get all users for the selected application
    /// </summary>
    Task<IList<IUser>> GetAll();

    /// <summary>
    /// Get all available users (with query)
    /// </summary>
    Task<ListResult<IUser>> GetByQuery(ListQuery<IUser> query);

    /// <summary>
    /// Creates or updates a user
    /// </summary>
    Task<EntityResult<IUser>> Save(IUser model);

    /// <summary>
    /// Deletes a user
    /// </summary>
    Task<EntityResult<IUser>> Delete(string id);

    /// <summary>
    /// Changes the password of the current user.
    /// User is logged out if this operation succeeds.
    /// </summary>
    Task<EntityResult<IUser>> UpdatePassword(IUser user, string currentPassword, string newPassword);

    /// <summary>
    /// Enables a user
    /// </summary>
    Task<EntityResult<IUser>> Enable(IUser user);

    /// <summary>
    /// Disables a user
    /// </summary>
    Task<EntityResult<IUser>> Disable(IUser user);
  }
}
