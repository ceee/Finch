using Raven.Client.Documents;
using System;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Models;
using zero.Core.Renderer;

namespace zero.Configuration;

public class SearchOptions : OptionsEnumerable<SearchIndexMap>, IOptionsEnumerable
{
  public bool IsEnabled { get; set; }


  public SearchOptions()
  {
    IsEnabled = true;
    //Map<Page>().Display((x, res, opts) =>
    //{
    //  PageType pageType = opts.Pages.GetByAlias(x.PageTypeAlias);
    //  if (pageType != null)
    //  {
    //    res.Icon = pageType.Icon;
    //  }
    //  res.Url = "/pages/edit/" + x.Id;
    //});
    //Map<MediaFolder>("fth-image");
  }


  public SearchIndexMap<T> Map<T>(string icon = null) where T : ZeroEntity, new()
  {
    SearchIndexMap<T> map = new(icon);
    Items.Add(map);
    return map;
  }
}


public class SearchIndexMap
{
  internal Type Type;
  internal string _Icon;
  protected string _group;
  protected string[] _fields;
  protected Func<ZeroEntity, SearchResult, IZeroOptions, Task> _modify;

  const string mapTemplate = @"map('{collection}', function (x) { 
                                return { 
                                  Id: x.Id,
                                  Group: '{group}',
                                  Name: x.Name,
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
      { "fields", BuildFieldArray(_fields) }
    });
  }

  internal string BuildFieldArray(string[] fields)
  {
    if (fields == null || !fields.Any())
    {
      return String.Empty;
    }

    return "x." + String.Join(", x.", fields);
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
  internal SearchIndexMap(string icon = null) : base(typeof(T), icon) {}

  public SearchIndexMap<T> Icon(string icon)
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
}
