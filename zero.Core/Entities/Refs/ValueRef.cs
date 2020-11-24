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
}
