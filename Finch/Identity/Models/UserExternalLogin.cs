using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Finch.Identity;

public class UserExternalLogin
{
  /// <summary>
  /// Gets or sets the provider for this instance of <see cref="UserExternalLogin"/>.
  /// </summary>
  public string LoginProvider { get; set; }

  /// <summary>
  /// Gets or sets the unique identifier for the user identity user provided by the login provider.
  /// </summary>
  public string ProviderKey { get; set; }

  /// <summary>
  /// Gets or sets the display name for the provider.
  /// </summary>
  public string ProviderDisplayName { get; set; }

  /// <summary>
  /// Convert to a UserLoginInfo
  /// </summary>
  public UserLoginInfo ToLoginInfo() => new(LoginProvider, ProviderKey, ProviderDisplayName);

  public UserExternalLogin() { }
}