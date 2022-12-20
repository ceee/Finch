using Raven.Client.Documents;
using System.Text.RegularExpressions;

namespace zero.Commerce.Numbers;

public class _NumberService : I_NumberService
{
  /// <inheritdoc />
  public IEnumerable<NumberType> RegisteredTypes { get; private set; }

  protected INumberStore Store { get; set; }

  const string DEFAULT_COUNTER = "Default";
  Regex templateRegex = new("{(date|number|year|random):?([a-zA-Z0-9-_]*)}", RegexOptions.IgnoreCase);
  Regex containsYearRegex = new("{(date|year|date:([^}]*y+[^}]*))}", RegexOptions.IgnoreCase);
  Random random = new();


  public _NumberService(INumberStore store, IZeroOptions options)
  {
    Store = store;
    RegisteredTypes = options.For<FlavorOptions>().GetAll<Number>().Select(x => x as NumberType);
  }


  /// <inheritdoc />
  public async Task<Number> Get(string alias)
  {
    NumberType type = RegisteredTypes.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    return await Load(type, () => type.Construct(type) as Number);
  }


  /// <inheritdoc />
  public async Task<T> Get<T>(string alias = null) where T : Number, new()
  {
    NumberType type = RegisteredTypes.FirstOrDefault(x => x.FlavorType == typeof(T) && (alias.IsNullOrEmpty() || x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase)));
    return await Load<T>(type);
  }


  /// <inheritdoc />
  public async Task<Paged<NumberCounter>> GetCounters(string alias) => await GetCounters(await Get(alias));


  /// <inheritdoc />
  public async Task<Paged<NumberCounter>> GetCounters<T>(string alias = null) where T : Number, new() => await GetCounters(await Get<T>(alias));


  protected async Task<Paged<NumberCounter>> GetCounters(Number number)
  {
    if (number == null || number.Hash.IsNullOrEmpty())
    {
      return new(0, 1, 1);
    }

    List<NumberCounter> items = new();
    Dictionary<string, long?> counters = await Store.Session.CountersFor(number).GetAllAsync();

    foreach (var counter in counters)
    {
      long value = counter.Value ?? 0;

      int.TryParse(counter.Key, out int year);
      bool isPassed = year > 0 && year < DateTimeOffset.Now.Year;

      items.Add(new()
      {
        Count = value,
        Key = counter.Key,
        Value = isPassed ? "@shop.number.fields.counterlist.yearpassed" : Compile(number, value + 1)
      });
    }

    return new(items, items.Count, 1, items.Count);
  }


  /// <inheritdoc />
  public async Task<Paged<Number>> GetAll(ListQuery<Number> query = default)
  {
    query ??= new();

    List<Number> result = new();
    List<Number> numbers = await Store.LoadAll();

    foreach (NumberType type in RegisteredTypes)
    {
      Number number = numbers.FirstOrDefault(x => x.Flavor == type.Alias);

      if (number == null)
      {
        number = type.Construct(type) as Number;
        number.Flavor = type.Alias;
      }

      number.Name = type.Name;
      number.IsActive = true;

      result.Add(number);
    }

    return new(result.Paging(query.Page, query.PageSize).ToList(), result.Count, query.Page, query.PageSize);
  }


  /// <inheritdoc />
  public async Task<string> Next<T>(string alias = null, DateTimeOffset? date = null) where T : Number, new()
  {
    T preset = await Get<T>(alias);

    if (!IsStored(preset))
    {
      NumberType type = RegisteredTypes.FirstOrDefault(x => x.FlavorType == typeof(T) && (alias.IsNullOrEmpty() || x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase)));
      preset = await Create<T>(type);
    }

    string counterName = GetCounterName(preset);

    // get a matching counter
    var counters = Store.Session.CountersFor(preset);
    long? storedCount = await counters.GetAsync(counterName);

    // calculate new value
    bool hasValue = storedCount.HasValue;
    long oldValue = hasValue ? storedCount.Value : preset.StartNumber;
    long incrementBy = hasValue ? 1 : 0;
    long newValue = oldValue + incrementBy;

    // increment the value and store it in database
    counters.Increment(counterName, hasValue ? incrementBy : newValue);
    await Store.Session.SaveChangesAsync();

    // compiles the template and returns the rendered result
    return Compile(preset, newValue, date);
  }


  /// <inheritdoc />
  public async Task Reset(string alias) => await Reset(await Get(alias));


  /// <inheritdoc />
  public async Task Reset<T>(string alias = null) where T : Number, new() => await Reset(await Get<T>(alias));


  /// <inheritdoc />
  protected async Task Reset(Number preset)
  {
    if (!IsStored(preset))
    {
      return;
    }

    string counterName = GetCounterName(preset);

    // get a matching counter
    var counters = Store.Session.CountersFor(preset);

    // delete the counter
    counters.Delete(counterName);
    await Store.Session.SaveChangesAsync();
  }


  /// <inheritdoc />
  public async Task<Result<Number>> Update(Number preset)
  {
    return IsStored(preset) ? await Store.Update(preset) : await Store.Create(preset);
  }


  /// <inheritdoc />
  public async Task Delete(string alias)
  {
    Number preset = await Get(alias);

    if (IsStored(preset))
    {
      await Store.Delete(preset);
    }
  }


  /// <inheritdoc />
  public async Task Delete<T>(string alias = null) where T : Number, new()
  {
    T preset = await Get<T>(alias);

    if (IsStored(preset))
    {
      await Store.Delete(preset);
    }
  }


  /// <inheritdoc />
  public string Compile<T>(T number, long value, DateTimeOffset? date = null) where T : Number
  {
    string output = number.Template;
    MatchCollection matches = templateRegex.Matches(output);
    DateTimeOffset usedDate = date ?? DateTimeOffset.Now;

    foreach (Match match in matches)
    {
      if (!match.Success)
      {
        continue;
      }

      string original = match.Value;
      string type = match.Groups[1].Value;
      string options = match.Groups[2].Value;
      string result = String.Empty;

      if (type == "year")
      {
        result = usedDate.Year.ToString();
      }
      else if (type == "date")
      {
        result = usedDate.ToString(options.IsNullOrWhiteSpace() ? "dd-MM-yyyy" : options);
      }
      else if (type == "number")
      {
        int minLength = number.MinLength > 0 && number.MinLength <= 16 ? number.MinLength : 1;
        result = value.ToString("D" + minLength);
      }
      else if (type == "random")
      {
        int length = 5;
        if (!options.IsNullOrWhiteSpace() && Int32.TryParse(options, out int oLength) && oLength > 0 && oLength <= 10)
        {
          length = oLength;
        }
        result = random.Next(0, Int32.MaxValue).ToString("D10").Substring(0, length);
      }

      output = output.ReplaceFirst(original, result);
    }

    return output;
  }


  /// <summary>
  /// Get integration data from database
  /// </summary>
  protected async Task<T> Load<T>(NumberType type, Func<T> createInstance = null) where T : Number, new()
  {
    if (type == null)
    {
      return default;
    }

    T entity = await Store.Session.Query<T>().FirstOrDefaultAsync(x => x.Flavor == type.Alias && x.IsActive);

    if (entity == null)
    {
      entity = createInstance != null ? createInstance() : new T();
    }

    entity.Name = type.Name;
    entity.IsActive = true;
    entity.Flavor = type.Alias;

    return entity;
  }


  /// <summary>
  /// Saves a new number to the database
  /// </summary>
  protected async Task<T> Create<T>(NumberType type, Action<T> modify = null) where T : Number, new()
  {
    T entity = new();
    entity.Flavor = type.Alias;
    entity.Name = null;
    entity.IsActive = true;

    modify?.Invoke(entity);

    Result<Number> result = await Store.Create(entity);

    if (!result.IsSuccess)
    {
      throw new Exception("Could not store number in database"); // TODO add details for logging
    }

    return (T)result.Model;
  }


  /// <summary>
  /// Get the name for the current counter
  /// </summary>
  protected virtual string GetCounterName<T>(T number) where T : Number
  {
    if (!number.ResetNumberPerYear || !containsYearRegex.IsMatch(number.Template))
    {
      return DEFAULT_COUNTER;
    }

    return DateTimeOffset.Now.Year.ToString();
  }


  /// <summary>
  /// Determines if this number comes from the database
  /// </summary>
  protected virtual bool IsStored<T>(T number) where T : Number
  {
    return number != null && !number.Hash.IsNullOrEmpty();
  }
}


public interface I_NumberService
{
  /// <summary>
  /// Get a number config by an alias
  /// </summary>
  Task<Number> Get(string alias);

  /// <summary>
  /// Get the number config of a certain type with an optional alias
  /// </summary>
  Task<T> Get<T>(string alias = null) where T : Number, new();

  /// <summary>
  /// Get all counters for a number
  /// </summary>
  Task<Paged<NumberCounter>> GetCounters(string alias);

  /// <summary>
  /// Get all counters for a number
  /// </summary>
  Task<Paged<NumberCounter>> GetCounters<T>(string alias = null) where T : Number, new();

  /// <summary>
  /// Get all numbers
  /// </summary>
  Task<Paged<Number>> GetAll(ListQuery<Number> query = default);

  /// <summary>
  /// Get the next available number for the specified template
  /// </summary>
  Task<string> Next<T>(string alias = null, DateTimeOffset? date = null) where T : Number, new();

  /// <summary>
  /// Resets the current counter for the specified template
  /// </summary>
  Task Reset(string alias);

  /// <summary>
  /// Resets the current counter for the specified template
  /// </summary>
  Task Reset<T>(string alias = null) where T : Number, new();

  /// <summary>
  /// Updates number properties
  /// </summary>
  Task<Result<Number>> Update(Number preset);

  /// <summary>
  /// Resets the whole number configuration including changes and all attached counters
  /// </summary>
  Task Delete(string alias);

  /// <summary>
  /// Resets the whole number configuration including changes and all attached counters
  /// </summary>
  Task Delete<T>(string alias = null) where T : Number, new();

  /// <summary>
  /// Renders the final output from the template and the value
  /// </summary>
  string Compile<T>(T number, long value, DateTimeOffset? date = null) where T : Number;
}