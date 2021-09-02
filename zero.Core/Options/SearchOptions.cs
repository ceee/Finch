using Raven.Client.Documents;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Models;
using zero.Core.Renderer;

namespace zero.Core.Options
{
  public class SearchOptions : ZeroBackofficeCollection<SearchIndexMap>, IZeroCollectionOptions
  {
    public bool IsEnabled { get; set; }


    public SearchOptions()
    {
      IsEnabled = true;
      Map<Page>();
      Map<MediaFolder>();
    }


    public SearchIndexMap<T> Map<T>(string group = null) where T : ZeroEntity, new()
    {
      SearchIndexMap<T> map = new(group);
      Items.Add(map);
      return map;
    }
  }


  public class SearchIndexMap
  {
    protected Type _type;
    protected string _group;
    protected string[] _fields;
    protected Func<ZeroEntity, SearchResult, Task> _modify;

    const string mapTemplate = @"map('{collection}', function (x) { 
                                  return { 
                                    Id: x.Id,
                                    Group: '{group}',
                                    Name: x.Name,
                                    IsActive: x.IsActive,
                                    Fields: [{fields}]
                                  };
                                });";

    internal SearchIndexMap(Type type, string group)
    {
      _type = type;
      _group = group;
    }

    internal string BuildInstruction(IZeroIndexDefinition index, IDocumentStore store)
    {
      return TokenReplacement.Apply(mapTemplate, new()
      {
        { "collection", store.Conventions.GetCollectionName(_type) },
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
  }


  public class SearchIndexMap<T> : SearchIndexMap where T : ZeroEntity
  {
    internal SearchIndexMap(string group) : base(typeof(T), group) {}

    public SearchIndexMap<T> Display(Action<T, SearchResult> modify = null)
    {
      _modify = (x, res) =>
      {
        modify?.Invoke(x as T, res);
        return Task.CompletedTask;
      };
      return this;
    }

    public SearchIndexMap<T> Display(Func<T, SearchResult, Task> modify = null)
    {
      _modify = (x, res) => modify?.Invoke(x as T, res);
      return this;
    }

    public SearchIndexMap<T> Fields(params string[] fieldNames)
    {
      _fields = fieldNames;
      return this;
    }
  }
}
