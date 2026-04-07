using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace Finch.Raven;

public class FinchTokenProvider : IFinchTokenProvider
{
  readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-".ToCharArray();

  readonly RandomNumberGenerator randonNumberGenerator;

  protected IFinchStore Store { get; }
    

  public FinchTokenProvider(IFinchStore store)
  {
    Store = store;
    randonNumberGenerator = RandomNumberGenerator.Create();
  }


  /// <inheritdoc />
  public virtual async Task<string> Create(string key, TimeSpan expires, int length = 82, Dictionary<string, string> metadata = default)
  {
    if (key.IsNullOrWhiteSpace())
    {
      throw new ArgumentException("Key cannot be empty");
    }
    if (length < 16)
    {
      throw new ArgumentOutOfRangeException("Use at least a length of 16 for the token");
    }

    string tokenKey = Random(length);

    SecurityToken securityToken = new()
    {
      Id = TokenToId(tokenKey),
      Token = tokenKey,
      Key = HashKey(key),
      Metadata = metadata ?? new()
    };

    IAsyncDocumentSession session = Store.Session();

    // saves the token
    await session.StoreAsync(securityToken);

    // set the expires flag for the token
    session.Expires(securityToken, expires);
    await session.SaveChangesAsync();

    return tokenKey;
  }


  /// <inheritdoc />
  public virtual async Task<bool> Verify(string key, string token)
  {
    if (token.IsNullOrWhiteSpace() || key.IsNullOrWhiteSpace())
    {
      return false;
    }

    IAsyncDocumentSession session = Store.Session();

    // try to find a valid token
    SecurityToken securityToken = await session.LoadAsync<SecurityToken>(TokenToId(token));
    bool isValid = securityToken != null && VerifyKey(securityToken.Key, key);

    // remove token from DB if it is valid
    if (isValid)
    {
      session.Delete(securityToken);
      await session.SaveChangesAsync();
    }

    return isValid;
  }


  /// <inheritdoc />
  public virtual async Task<SecurityToken> VerifyAndReturn(string key, string token)
  {
    if (token.IsNullOrWhiteSpace() || key.IsNullOrWhiteSpace())
    {
      return null;
    }

    IAsyncDocumentSession session = Store.Session();

    // try to find a valid token
    SecurityToken securityToken = await session.LoadAsync<SecurityToken>(TokenToId(token));
    bool isValid = securityToken != null && VerifyKey(securityToken.Key, key);

    // remove token from DB if it is valid
    if (isValid)
    {
      session.Delete(securityToken);
      await session.SaveChangesAsync();
      return securityToken;
    }

    return null;
  }


  /// <inheritdoc />
  public string Random(int length)
  {
    byte[] data = new byte[4 * length];
    randonNumberGenerator.GetBytes(data);

    StringBuilder result = new(length);

    for (int i = 0; i < length; i++)
    {
      var rnd = BitConverter.ToUInt32(data, i * 4);
      var idx = rnd % chars.Length;

      result.Append(chars[idx]);
    }

    return result.ToString();
  }


  /// <inheritdoc />
  public virtual async Task<bool> Exists(string token)
  {
    if (token.IsNullOrWhiteSpace())
    {
      return false;
    }

    IAsyncDocumentSession session = Store.Session();

    // try to find a valid token
    SecurityToken securityToken = await session.LoadAsync<SecurityToken>(TokenToId(token));
    return securityToken != null;
  }


  /// <summary>
  /// Converts the token to a database id
  /// </summary>
  string TokenToId(string token)
  {
    string collection = Store.Raven.Conventions.GetCollectionName(typeof(SecurityToken));
    string idPrefix = Store.Raven.Conventions.TransformTypeCollectionNameToDocumentIdPrefix(collection);
    return idPrefix + Store.Raven.Conventions.IdentityPartsSeparator + token;
  }


  /// <summary>
  /// Creates a hash for the security token key.
  /// Borrowed from the .NET Core PasswordHasher 
  /// (see: https://github.com/dotnet/aspnetcore/blob/release/5.0/src/Identity/Extensions.Core/src/PasswordHasher.cs)
  /// </summary>
  string HashKey(string key)
  {
    key = key.ToLowerInvariant();

    KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256;
    int iterationCount = 10000;
    int saltSize = 128 / 8;
    int numBytesRequested = 256 / 8;

    byte[] salt = new byte[saltSize];
    randonNumberGenerator.GetBytes(salt);
    byte[] subkey = KeyDerivation.Pbkdf2(key, salt, prf, iterationCount, numBytesRequested);

    var outputBytes = new byte[13 + salt.Length + subkey.Length];
    outputBytes[0] = 0x01;
    WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
    WriteNetworkByteOrder(outputBytes, 5, (uint)iterationCount);
    WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
    Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
    Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);
    return Convert.ToBase64String(outputBytes);
  }


  /// <summary>
  /// Verifies the given key.
  /// Borrowed from the .NET Core PasswordHasher 
  /// (see: https://github.com/dotnet/aspnetcore/blob/release/5.0/src/Identity/Extensions.Core/src/PasswordHasher.cs)
  /// </summary>
  bool VerifyKey(string storedKey, string key)
  {
    byte[] decodedStoredKey = Convert.FromBase64String(storedKey);

    try
    {
      KeyDerivationPrf prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedStoredKey, 1);
      int embeddedIterCount = (int)ReadNetworkByteOrder(decodedStoredKey, 5);
      int saltLength = (int)ReadNetworkByteOrder(decodedStoredKey, 9);

      // Read the salt: must be >= 128 bits
      if (saltLength < 128 / 8)
      {
        return false;
      }

      byte[] salt = new byte[saltLength];
      Buffer.BlockCopy(decodedStoredKey, 13, salt, 0, salt.Length);

      // Read the subkey (the rest of the payload): must be >= 128 bits
      int subkeyLength = decodedStoredKey.Length - 13 - salt.Length;
      if (subkeyLength < 128 / 8)
      {
        return false;
      }
      byte[] expectedSubkey = new byte[subkeyLength];
      Buffer.BlockCopy(decodedStoredKey, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

      // Hash the incoming key and verify it
      byte[] actualSubkey = KeyDerivation.Pbkdf2(key, salt, prf, embeddedIterCount, subkeyLength);

      return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
    }
    catch
    {
      return false;
    }
  }


  static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
  {
    buffer[offset + 0] = (byte)(value >> 24);
    buffer[offset + 1] = (byte)(value >> 16);
    buffer[offset + 2] = (byte)(value >> 8);
    buffer[offset + 3] = (byte)(value >> 0);
  }

  static uint ReadNetworkByteOrder(byte[] buffer, int offset)
  {
    return ((uint)(buffer[offset + 0]) << 24)
      | ((uint)(buffer[offset + 1]) << 16)
      | ((uint)(buffer[offset + 2]) << 8)
      | ((uint)(buffer[offset + 3]));
  }
}


public interface IFinchTokenProvider
{
  /// <summary>
  /// Generates a token for a <paramref name="key"/> with a specified <paramref name="expires"/> lifespan.
  /// </summary>
  /// <param name="key">The purpose the token will be used for. The key must match when verifying the token and should always be hidden from the user.</param>
  /// <param name="expires">The token will automatically expire after this timespan.</param>
  /// <param name="length">Length of the geneated token.</param> 
  /// <param name="metadata">Additional metadata to store with the token. This data can be retrieved on verification with <see cref="VerifyAndReturn(string, string)"/></param>
  /// <returns>
  /// The generated token with the specified <paramref name="length"/>.
  /// </returns>
  Task<string> Create(string key, TimeSpan expires, int length = 82, Dictionary<string, string> metadata = default);

  /// <summary>
  /// Validates the passed <paramref name="token"/> for the specified <paramref name="key"/>.
  /// </summary>
  /// <param name="key">The purpose the token was used for.</param>
  /// <param name="token">The previously generated token.</param>
  /// <returns>
  /// False, if the token could not be found or it has already expired.
  /// </returns>
  Task<bool> Verify(string key, string token);

  /// <summary>
  /// Validates the passed <paramref name="token"/> for the specified <paramref name="key"/>.
  /// </summary>
  /// <param name="key">The purpose the token was used for.</param>
  /// <param name="token">The previously generated token.</param>
  /// <returns>
  /// Null, if the token could not be found or it has already expired.
  /// </returns>
  Task<SecurityToken> VerifyAndReturn(string key, string token);

  /// <summary>
  /// Generates a random token with the specified <paramref name="length"/> using the RNGCryptoServiceProvider.
  /// </summary>
  /// <remarks>
  /// This method won't store the token in the database and can't be used for verification. Use <see cref="Create(string, TimeSpan, int)"/> instead.
  /// </remarks>
  string Random(int length);

  /// <summary>
  /// Determines whether a token exists in the database.
  /// </summary>
  Task<bool> Exists(string token);
}
