using System;

namespace zero.Core.Entities
{
  public class Ref<T> : Ref where T : IZeroIdEntity
  {
    public Ref() : base() { }
    public Ref(string id) : base(id) { }
  }


  public class Refs<T> : Refs where T : IZeroIdEntity
  {
    public Refs() : base() { }
    public Refs(params string[] ids) : base(ids) { }
  }


  public class Ref
  {
    public Ref() { }

    public Ref(string id)
    {
      Id = id;
    }

    public string Id { get; set; }


    public override string ToString()
    {
      return Id;
    }
  }


  public class Refs
  {
    public Refs() { }

    public Refs(params string[] ids)
    {
      Ids = ids;
    }

    public string[] Ids { get; set; } = new string[0] { };
  }
}
