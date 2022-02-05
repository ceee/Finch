using Raven.Client.Documents;

namespace zero.Search;

public class SearchIndexMap
{
  internal Type Type;
  internal string _Icon;
  protected string _group;
  protected string[] _fields;
  protected float _boost = 0;
  protected Func<ZeroEntity, SearchResult, IZeroOptions, Task> _modify;

  const string mapTemplate = @"map('{collection}', function (x) { 
                                return { 
                                  Id: x.Id,
                                  Group: '{group}',
                                  Name: boost(x.Name, {boost}),
                                  IsActive: x.IsActive,
                                  Fields: [{fields}]
                                };
                              });";

  internal SearchIndexMap(Type type, string icon = null)
  {
    Type = type;
    _group = "__TODO";
    _Icon = icon;
  }

  internal string BuildInstruction(IDocumentStore store)
  {
    return TokenReplacement.Apply(mapTemplate, new()
    {
      { "collection", store.Conventions.GetCollectionName(Type) },
      { "group", _group },
      { "fields", BuildFieldArray(_fields) },
      { "boost", _boost.ToString() }
    });
  }

  internal string BuildFieldArray(string[] fields)
  {
    if (fields == null || !fields.Any())
    {
      return String.Empty;
    }

    return String.Join(", ", fields.Select(x => $"x.{x}")); //$"boost(x.{x}, {_boost})"));
  }

  internal bool CanModify(Type type)
  {
    return Type.IsAssignableFrom(type);
  }

  internal async Task Modify(ZeroEntity entity, SearchResult result, IZeroOptions options)
  {
    if (_modify != null)
    {
      await _modify(entity, result, options);
    }
  }
}


public class SearchIndexMap<T> : SearchIndexMap where T : ZeroEntity
{
  internal SearchIndexMap(string icon = null) : base(typeof(T), icon) { }

  public virtual SearchIndexMap<T> Icon(string icon)
  {
    _Icon = icon;
    return this;
  }

  public SearchIndexMap<T> Display(Action<T, SearchResult> modify = null)
  {
    _modify = (x, res, opts) =>
    {
      modify?.Invoke(x as T, res);
      return Task.CompletedTask;
    };
    return this;
  }

  public SearchIndexMap<T> Display(Func<T, SearchResult, Task> modify = null)
  {
    _modify = (x, res, opts) => modify?.Invoke(x as T, res);
    return this;
  }

  public SearchIndexMap<T> Display(Action<T, SearchResult, IZeroOptions> modify = null)
  {
    _modify = (x, res, opts) =>
    {
      modify?.Invoke(x as T, res, opts);
      return Task.CompletedTask;
    };
    return this;
  }

  public SearchIndexMap<T> Display(Func<T, SearchResult, IZeroOptions, Task> modify = null)
  {
    _modify = (x, res, opts) => modify?.Invoke(x as T, res, opts);
    return this;
  }

  public SearchIndexMap<T> Fields(params string[] fieldNames)
  {
    _fields = fieldNames;
    return this;
  }

  public SearchIndexMap<T> Boost(float boostValue)
  {
    _boost = boostValue;
    return this;
  }
}
