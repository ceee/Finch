using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using System.Security.Claims;

namespace zero.Identity;

public class UserRolesService : IUserRolesService
{
  protected IHttpContextAccessor HttpContextAccessor { get; set; }

  protected UserManager<ZeroUser> UserManager { get; private set; }

  protected RoleManager<ZeroUserRole> RoleManager { get; private set; }

  private ClaimsPrincipal Principal => HttpContextAccessor.HttpContext?.User;

  protected IZeroContext Context { get; private set; }

  protected IZeroDocumentSession Session => Context.Store.Session();


  public UserRolesService(IZeroContext context, IHttpContextAccessor httpContextAccessor, UserManager<ZeroUser> userManager, RoleManager<ZeroUserRole> roleManager, IStoreContext store)
  {
    Context = context;
    HttpContextAccessor = httpContextAccessor;
    UserManager = userManager;
    RoleManager = roleManager;
  }


  /// <inheritdoc />
  public async Task<IList<ZeroUserRole>> GetAll()
  {
    return await Session.Query<ZeroUserRole>().OrderBy(x => x.Sort).ThenBy(x => x.Name).ToListAsync();
  }


  /// <inheritdoc />
  public async Task<ZeroUserRole> GetById(string id)
  {
    return await RoleManager.FindByIdAsync(id);
  }


  /// <inheritdoc />
  public async Task<EntityResult<ZeroUserRole>> Save(ZeroUserRole model)
  {
    //ValidationResult validation = await new UserRoleValidator().ValidateAsync(model);

    //if (!validation.IsValid)
    //{
    //  return EntityResult<ZeroUserRole>.Fail(validation);
    //}

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

    return EntityResult<ZeroUserRole>.Success(model);
  }


  /// <inheritdoc />
  public async Task<EntityResult<ZeroUserRole>> Delete(string id)
  {
    ZeroUserRole country = await Session.LoadAsync<ZeroUserRole>(id);

    if (country == null)
    {
      return EntityResult<ZeroUserRole>.Fail("@errors.ondelete.idnotfound");
    }

    Session.Delete(country);

    await Session.SaveChangesAsync();

    return EntityResult<ZeroUserRole>.Success();
  }
}


public interface IUserRolesService
{
  /// <summary>
  /// Get all user roles
  /// </summary>
  Task<IList<ZeroUserRole>> GetAll();

  /// <summary>
  /// Get role by id
  /// </summary>
  Task<ZeroUserRole> GetById(string id);

  /// <summary>
  /// Create or update a role
  /// </summary>
  Task<EntityResult<ZeroUserRole>> Save(ZeroUserRole model);

  /// <summary>
  /// Deletes a role
  /// </summary>
  Task<EntityResult<ZeroUserRole>> Delete(string id);
}