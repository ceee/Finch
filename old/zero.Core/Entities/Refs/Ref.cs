namespace zero.Core.Entities
{
  public class Ref<T> : Ref where T : ZeroIdEntity
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
