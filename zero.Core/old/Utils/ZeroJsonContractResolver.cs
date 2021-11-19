using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace zero.Core.Utils
{
  public class ZeroJsonContractResolver : CamelCasePropertyNamesContractResolver
  {
    public override JsonContract ResolveContract(Type type)
    {
      JsonContract contract = base.ResolveContract(type);

      if (type.IsInterface)
      {
        //contract.
      }

      return contract;
    }


    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      return base.CreateProperty(member, memberSerialization);
    }
  }
}
