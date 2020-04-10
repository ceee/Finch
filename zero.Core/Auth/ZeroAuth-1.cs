//using FluentValidation.Results;
//using Raven.Client.Documents;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using zero.Core.Entities;
//using zero.Core.Validation;
//using zero.Core.Extensions;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Raven.Client.Documents.Session;
//using Microsoft.AspNetCore.Identity;

//namespace zero.Core.Auth
//{
//  public class ZeroAuth<TUser, TRole>
//    where TUser : class, IUser, new()
//    where TRole : class, IUserRole, new()
//  {
//    /// <summary>
//    /// Gets the database context for this store.
//    /// </summary>
//    protected IDocumentStore Raven { get; set; }

//    /// <summary>
//    /// Configuration
//    /// </summary>
//    protected IZeroConfiguration Config { get; private set; }

//    /// <summary>
//    /// Currently logged-in user
//    /// </summary>
//    protected IUser Current { get; private set; }

//    protected IHttpContextAccessor HttpContextAccessor { get; set; }

//    protected SignInManager<TUser> SignInManager { get; set; }


//    public ZeroAuth(IDocumentStore raven, IZeroConfiguration config, IUser currentUser, IHttpContextAccessor httpContextAccessor, SignInManager<TUser> signInManager)
//    {
//      Raven = raven;
//      Config = config;
//      HttpContextAccessor = httpContextAccessor;
//      SignInManager = signInManager;
//    }


//    /// <summary>
//    /// Tries to log in a user
//    /// </summary>
//    public async Task<TUser> Login(string username, string password, bool rememberMe = true)
//    {
//      ClaimsPrincipal principal = await ResolveClaimsPrincipalFromLogin(username, password);

//      if (principal != null && principal.Identity.IsAuthenticated)
//      {
//        await SignInManager.SignInAsync()
//        await HttpContextAccessor.HttpContext.SignInAsync(Constants.Auth.Scheme, principal, new AuthenticationProperties()
//        {
//          IsPersistent = rememberMe,
//          AllowRefresh = true,
//          ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
//        });
//      }

//      // error signing in
//      //if (principal == null || !principal.Identity.IsAuthenticated)
//      //{
//      //  return null;
//      //}

//      //return await ResolveUserFromClaimsPrincipal(principal);
//    }



//    /// <summary>
//    /// Try to create a claims principal from a login
//    /// </summary>
//    async Task<ClaimsPrincipal> ResolveClaimsPrincipalFromLogin(string email, string password)
//    {
//      TUser user = default;

//      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
//      {
//        user = await session.Query<TUser>().FirstOrDefaultAsync(query => query.Email == email);
//      }

//      // wrong username
//      if (user == null)
//      {
//        return null;
//      }

//      // generate password hash
//      //string passwordHash = BCrypt.Net.BCrypt.HashPassword(password, user.PasswordSalt);

//      //// wrong password
//      //if (!user.PasswordHash.Equals(passwordHash))
//      //{
//      //  return null;
//      //}

//      // create claims identity
//      //ClaimsIdentity identity = new ClaimsIdentity(new List<Claim>()
//      //{
//      //  new Claim(ClaimTypes.Name, user.Username),
//      //  new Claim(ClaimTypeDbId, user.Id)
//      //}, "SecureLogin");

//      //return new ClaimsPrincipal(identity);
//    }


//    /// <summary>
//    /// Try to get an associated user for a claims principal
//    /// </summary>
//    async Task<User> ResolveUserFromClaimsPrincipal(ClaimsPrincipal principal)
//    {
//      if (principal == null || !principal.Identity.IsAuthenticated)
//      {
//        return null;
//      }

//      Claim idClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypeDbId);

//      if (idClaim == null)
//      {
//        return null;
//      }

//      return await StoreApi.GetByIdAsync<User>(idClaim.Value);
//    }


//    /// <summary>
//    /// Try to get an associated user ID for a claims principal
//    /// </summary>
//    string ResolveIdFromClaimsPrincipal(ClaimsPrincipal principal)
//    {
//      if (principal == null || !principal.Identity.IsAuthenticated)
//      {
//        return null;
//      }

//      Claim idClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypeDbId);

//      if (idClaim == null)
//      {
//        return null;
//      }

//      return idClaim.Value;
//    }



//    /// <summary>
//    /// Adds a new backoffice user
//    /// </summary>
//    public async Task<EntityChangeResult<TUser>> Add(TUser model, CancellationToken token = default)
//    {
//      // set default language
//      if (model.LanguageId.IsNullOrEmpty())
//      {
//        model.LanguageId = Config.DefaultLanguage;
//      }

//      // validate model
//      ValidationResult validation = await new BackofficeUserValidator(isCreate: true).ValidateAsync(model);

//      if (!validation.IsValid)
//      {
//        return EntityChangeResult<TUser>.Fail(validation);
//      }

//      return EntityChangeResult<TUser>.Success();
//    }
//  }
//}
