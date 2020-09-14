using Newtonsoft.Json.Serialization;
using System;

namespace zero.Core.Utils
{
  public class ZeroJsonContractResolver : CamelCasePropertyNamesContractResolver
  {
    public override JsonContract ResolveContract(Type type)
    {
      return base.ResolveContract(type);
    }
  }
}
