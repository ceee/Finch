using Newtonsoft.Json.Serialization;
using System;

namespace zero.Core.Utils
{
  public class ZeroJsonContractResolver : DefaultContractResolver
  {
    public override JsonContract ResolveContract(Type type)
    {
      var resolver = new CamelCasePropertyNamesContractResolver();
      return resolver.ResolveContract(type);
    }
  }
}
