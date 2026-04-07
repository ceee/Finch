using Finch.Rendering.QrCode;

namespace Finch.Identity;

public class TwoFactorKey
{
  public string UserEmail { get; init; }

  public string UnformattedKey { get; init; }

  public string FormattedKey { get; init; }

  public string AuthenticatorUrl { get; init; }
  
  public QrCode QrCode { get; set; }

  public TwoFactorKeyOptions Options { get; init; }
}

public class TwoFactorKeyOptions
{
  public string Issuer { get; set; }
  
  public TwoFactorKeyLength Digits { get; set; } = TwoFactorKeyLength.Six;

  public TwoFactorKeyAlgorithm Algorithm { get; set; } = TwoFactorKeyAlgorithm.SHA1;

  public TwoFactorKeyRefreshPeriod Period { get; set; } = TwoFactorKeyRefreshPeriod.ThirtySeconds;
}

public enum TwoFactorKeyLength
{
  Six = 6,
  Seven = 7,
  Eight = 8
}

public enum TwoFactorKeyAlgorithm
{
  // ReSharper disable once InconsistentNaming
  SHA1 = 0, 
  // ReSharper disable once InconsistentNaming
  SHA256 = 1,
  // ReSharper disable once InconsistentNaming
  SHA512 = 2
}

public enum TwoFactorKeyRefreshPeriod
{
  FifteenSeconds = 15,
  ThirtySeconds = 30,
  SixtySeconds = 60
}