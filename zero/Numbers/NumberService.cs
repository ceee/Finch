using System.Text.RegularExpressions;
using zero.Numbers;

namespace zero.Numbers;

public class NumberService : INumberService
{
  protected IZeroNumberStoreDbProvider Db { get; set; }

  const string DEFAULT_COUNTER = "Default";
  Regex templateRegex = new("{(date|number|year|random):?([a-zA-Z0-9-_]*)}", RegexOptions.IgnoreCase);
  Regex containsYearRegex = new("{(date|year|date:([^}]*y+[^}]*))}", RegexOptions.IgnoreCase);
  Random random = new();


  public NumberService(IZeroNumberStoreDbProvider db, IZeroOptions options)
  {
    Db = db;
    //RegisteredTypes = options.For<FlavorOptions>().GetAll<Number>().Select(x => x as NumberType);
  }
}


public interface INumberService
{
  /// <summary>
  /// Get the next available number for the specified template
  /// </summary>
  Task<string> Next(string alias, DateTimeOffset? date = null, bool store = true);

  /// <summary>
  /// Resets the current counter for the specified template
  /// </summary>
  Task Reset(string alias);

  /// <summary>
  /// Resets all counters for the specified template
  /// </summary>
  Task ResetAll(string alias);

  /// <summary>
  /// Renders the final output from the template and the value
  /// </summary>
  string Compile(string alias, long value, DateTimeOffset? date = null);
}