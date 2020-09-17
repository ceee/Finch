using System;

namespace zero.Core.Entities
{
  public class Ref<T> where T : IZeroIdEntity
  {
    public Ref() { }

    public Ref(string id)
    {
      Id = id;
    }

    public string Id { get; private set; }


    public override string ToString()
    {
      return Id;
    }
  }


  public class Refs<T> where T : IZeroIdEntity
  {
    public Refs() { }

    public Refs(params string[] ids)
    {
      Ids = ids;
    }

    public string[] Ids { get; private set; } = new string[0] { };
  }
}
