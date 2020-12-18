using System.Globalization;
using System.Threading.Tasks;

namespace zero.Core.Tokens
{
  public abstract class ZeroTokenProvider<T>
  {
    /// <summary>
    /// Creates bytes to use as a security token from the models internal data (preferable security stamp).
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>The security token bytes.</returns>
    protected abstract Task<byte[]> GetSecurityToken(T model);


    /// <summary>
    /// Returns a constant, provider and model unique modifier used for entropy in generated tokens from model information.
    /// </summary>
    /// <param name="purpose">The purpose the token will be generated for.</param>
    /// <param name="model">The model a token should be generated for.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing a constant modifier for the specified 
    /// <paramref name="model"/> and <paramref name="purpose"/>.
    /// </returns>
    protected abstract Task<string> GetModifier(string purpose, T model);


    /// <inheritdoc />
    public virtual async Task<string> Generate(string purpose, T model)
    {
      var securityToken = await GetSecurityToken(model);
      var modifier = await GetModifier(purpose, model);

      return Rfc6238AuthenticationService.GenerateCode(securityToken, modifier).ToString("D6", CultureInfo.InvariantCulture);
    }


    /// <inheritdoc />
    public virtual async Task<bool> Validate(string purpose, string token, T model)
    {
      int code;
      if (!int.TryParse(token, out code))
      {
        return false;
      }
      var securityToken = await GetSecurityToken(model);
      var modifier = await GetModifier(purpose, model);

      return securityToken != null && Rfc6238AuthenticationService.ValidateCode(securityToken, code, modifier);
    }
  }


  public interface IZeroTokenProvider<T>
  {
    /// <summary>
    /// Generates a token for the specified <paramref name="model"/> and <paramref name="purpose"/>.
    /// </summary>
    /// <param name="purpose">The purpose the token will be used for.</param>
    /// <param name="model">The model a token should be generated for.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the token for the specified 
    /// <paramref name="model"/> and <paramref name="purpose"/>.
    /// </returns>
    /// <remarks>
    /// The <paramref name="purpose"/> parameter allows a token generator to be used for multiple types of token whilst
    /// insuring a token for one purpose cannot be used for another.
    /// </remarks>
    Task<string> Generate(string purpose, T model);

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="token"/> is valid for the given
    /// <paramref name="model"/> and <paramref name="purpose"/>.
    /// </summary>
    /// <param name="purpose">The purpose the token will be used for.</param>
    /// <param name="token">The token to validate.</param>
    /// <param name="model">The model a token should be validated for.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the a flag indicating the result
    /// of validating the <paramref name="token"> for the specified </paramref><paramref name="model"/> and <paramref name="purpose"/>.
    /// The task will return true if the token is valid, otherwise false.
    /// </returns>
    Task<bool> Validate(string purpose, string token, T model);
  }
}
