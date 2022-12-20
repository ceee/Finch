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


  /// <inheritdoc />
  public async Task<string> Next(string alias, DateTimeOffset? date = null, bool store = true)
  {
    Number number = await Get(alias);
    throw new NotImplementedException();
  }


  /// <inheritdoc />
  public Task Reset(string alias)
  {
    throw new NotImplementedException();
  }


  /// <inheritdoc />
  public Task ResetAll(string alias)
  {
    throw new NotImplementedException();
  }


  /// <inheritdoc />
  public string Compile(string alias, long value, DateTimeOffset? date = null)
  {
    throw new NotImplementedException();
  }


  /// <summary>
  /// Create a new number implementation
  /// </summary>
  protected async Task<Number> Get(string alias, string template = "{number}", long startNumber = 1, int minLength = 1)
  {
    string id = Id(alias);

    Number number = await Db.Load<Number>(id);
    bool exists = number != null;

    if (!exists)
    {
      number = new()
      {
        Id = id,
        Template = template,
        StartNumber = startNumber,
        MinLength = minLength
      };

      await Db.Create(number);
    }
    else
    {
      bool changed = false;

      if (!number.Template.Equals(template))
      {
        number.Template = template;
        changed = true;
      }
      if (!number.StartNumber.Equals(startNumber))
      {
        number.StartNumber = startNumber;
        changed = true;
      }
      if (!number.MinLength.Equals(minLength))
      {
        number.MinLength = minLength;
        changed = true;
      }

      if (changed)
      {
        await Db.Update(number);
      }
    }

    return number;
  }


  /// <summary>
  /// Generate number ID
  /// </summary>
  protected string Id(string alias)
  {
    return "numbers." + alias;
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