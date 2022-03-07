using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace zero.Preview;

public class PreviewService : IPreviewService
{
  protected IZeroTokenProvider TokenProvider { get; private set; }

  protected IZeroOptions Options { get; private set; }


  public PreviewService(IZeroTokenProvider tokenProvider, IZeroOptions options)
  {
    TokenProvider = tokenProvider;
    Options = options;
  }


  /// <inheritdoc />
  public async Task<string> CreateAccessToken(string key, ClaimsPrincipal backofficeUser)
  {
    string fullKey = GetKey(key, backofficeUser);
    return await TokenProvider.Create(fullKey, TimeSpan.FromMinutes(Options.For<PreviewOptions>().TokenExpirationInMinutes), 32);
  }


  /// <inheritdoc />
  public async Task<bool> IsAccessGranted(HttpRequest request, ClaimsPrincipal backofficeUser)
  {
    string token = request.Query[Options.For<PreviewOptions>().QueryParameter];

    if (!token.HasValue())
    {
      return false;
    }

    return await IsAccessGranted(token, backofficeUser);
  }


  /// <inheritdoc />
  public async Task<bool> IsAccessGranted(string token, ClaimsPrincipal backofficeUser)
  {
    return await TokenProvider.Exists(token);
  }


  /// <summary>
  /// Generate the key for a new preview access
  /// </summary>
  protected string GetKey(string keyPrefix, ClaimsPrincipal backofficeUser)
  {
    //string userId = UserManager.GetUserId(backofficeUser);

    //if (userId.IsNullOrEmpty())
    //{
    //  throw new InvalidOperationException("Preview access is only granted for authenticated users");
    //}

    if (keyPrefix.IsNullOrEmpty())
    {
      throw new ArgumentNullException(nameof(keyPrefix), "Please define a key for generating preview access");
    }

    return "zero:preview:" + /*userId + ":" + */ keyPrefix;
  }
}

public interface IPreviewService
{
  /// <summary>
  /// Create a token for a new preview access.
  /// </summary>
  Task<string> CreateAccessToken(string key, ClaimsPrincipal backofficeUser);

  // <summary>
  /// Determine whether the access to the preview is granted based on an HTTP request
  /// </summary>
  Task<bool> IsAccessGranted(HttpRequest request, ClaimsPrincipal backofficeUser);

  /// <summary>
  /// Determine whether the access to the preview is granted based on the passed token
  /// </summary>
  Task<bool> IsAccessGranted(string token, ClaimsPrincipal backofficeUser);
}