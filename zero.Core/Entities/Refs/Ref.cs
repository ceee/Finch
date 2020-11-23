using Newtonsoft.Json;
using zero.Core.Extensions;

namespace zero.Core.Entities
{
  public class Ref
  {
    public string Id { get; set; }

    [JsonProperty("Core")]
    public bool IsCore { get; set; }

    public Ref() { }

    public Ref(string id) : this()
    {
      Id = id;
    }

    public Ref(string id, bool isCore) : this(id)
    {
      IsCore = isCore;
    }


    public override string ToString()
    {
      return (IsCore ? "core:" : string.Empty) + Id;
    }

    public static Ref Create(string id, bool isCore = false)
    {
      return id.IsNullOrWhiteSpace() ? null : new Ref(id, isCore);
    }
  }
}
