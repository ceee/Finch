namespace zero.Core.Entities
{
  public class ValueRef<T> : ValueRef<T, string> where T : IZeroIdEntity
  {
    public ValueRef() : base() { }
    public ValueRef(string id, string value) : base(id, value) { }

    public static implicit operator string(ValueRef<T> reference) => reference.Id;
  }


  public class ValueRef<TEntity, TValue> : Ref where TEntity : IZeroIdEntity
  {
    public ValueRef() : base() { }
    public ValueRef(string id, TValue value) : base(id)
    {
      Value = value;
    }

    public TValue Value { get; set; }

    public static implicit operator string(ValueRef<TEntity, TValue> reference) => reference.Id;
  }


  //public class ValueRef : Ref
  //{
  //  public ValueRef() : base() { }

  //  public ValueRef(string id, string value) : base(id)
  //  {
  //    Value = value;
  //  }

  //  public string Value { get; set; }


  //  public override string ToString()
  //  {
  //    return (Id, Value).ToString();
  //  }
  //}


  public class Ref<T> : Ref where T : IZeroIdEntity
  {
    public Ref() : base() { }
    public Ref(string id) : base(id) { }

    public static implicit operator Ref<T>(string id) => new Ref<T>(id);

    public static implicit operator string(Ref<T> reference) => reference.Id;
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


    public static implicit operator Ref(string id) => new Ref(id);

    public static implicit operator string(Ref reference) => reference.Id;
  }
}
