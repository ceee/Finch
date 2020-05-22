using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using zero.Core.Renderer;

namespace zero.Core
{
  public static class EntityMap
  {
    static Dictionary<Type, Type> Types = new Dictionary<Type, Type>();

    public static void Use<TService, TImplementation>() where TImplementation : TService
    {
      Types[typeof(TService)] = typeof(TImplementation);
    }


    public static Type Get<T>() => Get(typeof(T));


    public static Type Get(Type type)
    {
      if (!Types.ContainsKey(type))
      {
        return null;
      }

      return Types[type];
    }
  }


  public class EntityDefinition<TService, TImplementation> where TImplementation : TService
  {
    public Type ServiceType => typeof(TService);

    public Type ImplementationType => typeof(TImplementation);

    public IList<IValidator> Validators { get; set; } = new List<IValidator>();

    public AbstractRenderer<TImplementation> Renderer { get; set; }
  }

  public class EntityDefinition
  {
    public Type ServiceType { get; protected set; }

    public Type ImplementationType { get; protected set; }

    public IList<IValidator> Validators { get; set; } = new List<IValidator>();

    public AbstractGenericRenderer Renderer { get; set; }

    public static EntityDefinition Convert<TService, TImplementation>(EntityDefinition<TService, TImplementation> definition) where TImplementation : TService
    {
      return new EntityDefinition()
      {
        ServiceType = definition.ServiceType,
        ImplementationType = definition.ImplementationType,
        Validators = definition.Validators,
        Renderer = definition.Renderer.ToGenericRenderer()
      };
    }
  }
}
