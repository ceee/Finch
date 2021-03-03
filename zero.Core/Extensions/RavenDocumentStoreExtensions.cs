using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Options;
using zero.Core.Utils;

namespace zero.Core.Extensions
{
  public static class RavenDocumentStoreExtensions
  {
    const char DOT = '.';

    /// <summary>
    /// Setup conventions for the document store
    /// </summary>
    public static IDocumentStore Setup(this IDocumentStore store, IZeroOptions options)
    {
      Type[] polymorphTypes = new Type[2] { typeof(SpaceContent), typeof(Page) };

      Type dbConventionType = typeof(IZeroDbConventions);

      store.Conventions.IdentityPartsSeparator = DOT;

      store.Conventions.RegisterAsyncIdConvention<IZeroEntity>((_, entity) =>
      {
        string guid = IdGenerator.Create((entity is IBackofficeUser or IBackofficeUserRole) ? 8 : -1);
        string collection = store.Conventions.GetCollectionName(entity);

        string tag = store.Conventions.TransformTypeCollectionNameToDocumentIdPrefix(collection);
        return Task.FromResult(tag + store.Conventions.IdentityPartsSeparator + guid);
      });

      store.Conventions.RegisterAsyncIdConvention<IPage>((_, entity) =>
      {
        return store.Conventions.AsyncDocumentIdGenerator(_, entity);
      });

      (store.Conventions.Serialization as NewtonsoftJsonSerializationConventions).CustomizeJsonDeserializer = x =>
      {
        x.Converters.Add(new RefJsonConverter());
      };
      (store.Conventions.Serialization as NewtonsoftJsonSerializationConventions).CustomizeJsonSerializer = x =>
      {
        x.Converters.Add(new RefJsonConverter());
      };

      store.Conventions.FindCollectionName = type =>
      {
        Type finalType = type;

        // do not alter non-internal entities
        if (!dbConventionType.IsAssignableFrom(type))
        {
          return DocumentConventions.DefaultGetCollectionName(type);
        }

        // use name from attribute if available
        CollectionAttribute collection = type.GetCustomAttribute<CollectionAttribute>(true);
        if (collection != null)
        {
          return collection.Name;
        }

        // use base interface if available
        Type interfaceBaseType = type.GetInterfaces().FirstOrDefault(x => x.IsInterface && dbConventionType.IsAssignableFrom(x) && x.Name != dbConventionType.Name);
        
        if (interfaceBaseType != null)
        {
          // use name from attribute if available
          collection = interfaceBaseType.GetCustomAttribute<CollectionAttribute>(true);
          if (collection != null)
          {
            return options.Raven.CollectionPrefix + collection.Name;
          }
        }

        // use base type for polymorphism
        Type polymorphBaseType = polymorphTypes.FirstOrDefault(x => type.IsSubclassOf(x));

        if (polymorphBaseType != null)
        {
          finalType = polymorphBaseType;
        }

        return options.Raven.CollectionPrefix + DocumentConventions.DefaultGetCollectionName(finalType);
      };


      store.Conventions.TransformTypeCollectionNameToDocumentIdPrefix = name =>
      {
        if (!options.Raven.CollectionPrefix.IsNullOrWhiteSpace())
        {
          name = options.Raven.CollectionPrefix.EnsureEndsWith(store.Conventions.IdentityPartsSeparator) + name.TrimStart(options.Raven.CollectionPrefix);
        }

        return name.ToCamelCaseId();
      };

      return store;
    }
  }
}
