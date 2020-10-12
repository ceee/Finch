using System;

namespace zero.Core.Entities
{
  public class Ref<T> : Ref where T : IZeroIdEntity
  {
    public Ref() : base() { }
    public Ref(string id) : base(id) { }
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
}
