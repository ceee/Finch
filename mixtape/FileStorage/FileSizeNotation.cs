namespace Mixtape.FileStorage;

public enum FileSizeNotation
{
  /// <summary>
  /// Multiply by 1.000, as specified by the International System of Units (e.g. kB, MB, GB)
  /// </summary>
  SI = 0,
  /// <summary>
  /// Binary notation with a multiplier of 1.024 (e.g. KiB, MiB, GiB)
  /// </summary>
  IEC = 1,
  /// <summary>
  /// Binary notation with a multiplier of 1.024 (e.g. KB, MB, GB).
  /// Ends with GB, there is no TB, ...
  /// </summary>
  JEDEC = 2
}
