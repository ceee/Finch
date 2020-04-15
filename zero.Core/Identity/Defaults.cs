using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Core.Identity
{
  public class ZeroUserStore : UserStore<User>
  {
    public ZeroUserStore(IDocumentStore raven) : base(raven) { }
  }

  public class ZeroRoleStore : RoleStore<UserRole>
  {
    public ZeroRoleStore(IDocumentStore raven, IdentityErrorDescriber describer = null) : base(raven, describer) { }
  }


  //public class ZeroUserManager : UserManager<User>
  //{
  //  public ZeroUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher,
  //    IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators,
  //    ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
  //    : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger) { }
  //}


  //public class ZeroSignInManager : SignInManager<User>
  //{
  //  public ZeroSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory,
  //    IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<User>> logger, IAuthenticationSchemeProvider schemes)
  //    : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes) { }
  //}


  //public class ZeroRoleManager : RoleManager<UserRole>
  //{
  //  public ZeroRoleManager(IRoleStore<UserRole> store, IEnumerable<IRoleValidator<UserRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<UserRole>> logger)
  //    : base(store, roleValidators, keyNormalizer, errors, logger) { }
  //}

}