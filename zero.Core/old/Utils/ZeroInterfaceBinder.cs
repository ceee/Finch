using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace zero.Core.Utils
{
  public class ZeroInterfaceBinder : ISerializationBinder
  {
    public IList<Type> KnownTypes { get; set; }

    Dictionary<string, Type> KnownTypesDictionary { get; } = new Dictionary<string, Type>();



    public Type BindToType(string assemblyName, string typeName)
    {
      return KnownTypesDictionary.SingleOrDefault(x => x.Key == typeName).Value;
    }


    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
      string id = null;

      if (!KnownTypesDictionary.ContainsValue(serializedType))
      {
        id = IdGenerator.Create();
        KnownTypesDictionary.Add(id, serializedType);
      }
      else
      {
        id = KnownTypesDictionary.SingleOrDefault(x => x.Value == serializedType).Key;
      }

      assemblyName = null;
      typeName = id;
    }
  }
}

