using System.Globalization;

namespace zero.Localization;

public class CultureService : ICultureService
{
  /// <inheritdoc />
  public List<Culture> GetAllCultures(params string[] codes)
  {
    return CultureInfo.GetCultures(CultureTypes.AllCultures)
      .Where(x => !x.Name.IsNullOrWhiteSpace())
      .Select(x => new CultureInfo(x.Name))
      .Where(x => codes.Length > 0 ? codes.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase) : true)
      .OrderBy(x => x.DisplayName)
      .Select(x => new Culture()
      {
        Code = x.Name,
        Name = x.DisplayName
      })
      .ToList();
  }
}


public interface ICultureService
{
  /// <summary>
  /// Get all available cultures
  /// </summary>
  List<Culture> GetAllCultures(params string[] codes);
}