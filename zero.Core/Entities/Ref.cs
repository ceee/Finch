using System;
using zero.Core.Extensions;

namespace zero.Core.Entities
{
  public class Ref<T> : Ref where T : IZeroIdEntity
  {
    public Ref() : base() { }
    public Ref(string id) : base(id) { }

    public static implicit operator string(Ref<T> obj) => obj.Id;
    public static explicit operator Ref<T>(string obj) => new Ref<T>(obj);
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


    public bool Equals(Ref other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(Id, other.Id, StringComparison.InvariantCultureIgnoreCase);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return ReferenceEquals(this, obj) || Equals((Ref)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return StringComparer.InvariantCultureIgnoreCase.GetHashCode(Id);
      }
    }

    public static bool operator ==(Ref left, Ref right) => Equals(left, right);
    public static bool operator !=(Ref left, Ref right) => !Equals(left, right);
    public static bool operator ==(Ref left, string right) => string.Equals(left?.Id, right, StringComparison.InvariantCultureIgnoreCase);
    public static bool operator !=(Ref left, string right) => !string.Equals(left?.Id, right, StringComparison.InvariantCultureIgnoreCase);
    public static bool operator ==(string left, Ref right) => string.Equals(left, right?.Id, StringComparison.InvariantCultureIgnoreCase);
    public static bool operator !=(string left, Ref right) => !string.Equals(left, right?.Id, StringComparison.InvariantCultureIgnoreCase);

    public static implicit operator string(Ref obj) => obj.Id;
    public static explicit operator Ref(string obj) => new Ref(obj);
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


  public static class RefExtensions
  {
    public static Ref<T> Ref<T>(this string str) where T : IZeroIdEntity
    {
      if (str.IsNullOrEmpty())
      {
        return null;
      }
      return new Ref<T>(str);
    }
  }
}
