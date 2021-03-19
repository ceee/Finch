using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Subscriptions;
using Raven.Client.Json.Serialization;
using Raven.Client.Json.Serialization.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Core.Utils;

namespace zero.Core.Database
{
  public class ZeroDocumentConventionsBuilder : IZeroDocumentConventionsBuilder
  {
    protected HashSet<Type> PolymorphTypes { get; private set; } = new();

    protected Type AcceptsZeroConventionsType { get; set; } = typeof(IZeroDbConventions);

    protected char IdentityPartsSeparator { get; set; } = '.';

    protected IZeroOptions Options { get; private set; }

    protected static Dictionary<Type, string> CachedTypeCollectionNameMap = new();


    public ZeroDocumentConventionsBuilder(IZeroOptions options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public void Run(DocumentConventions conventions)
    {
      conventions.MaxNumberOfRequestsPerSession = 100;
      conventions.IdentityPartsSeparator = IdentityPartsSeparator;
      conventions.TransformTypeCollectionNameToDocumentIdPrefix = TransformTypeCollectionNameToDocumentIdPrefix;
      conventions.FindCollectionName = FindCollectionName;
      conventions.RegisterAsyncIdConvention<IZeroEntity>((_, entity) => GetDocumentId(conventions, entity));

      ConfigureJsonSerializer(conventions.Serialization);
    }


    /// <summary>
    /// Get a document ID from an entity
    /// </summary>
    protected virtual Task<string> GetDocumentId<T>(DocumentConventions conventions, T entity)
    {
      string collection = conventions.GetCollectionName(entity);

      StringBuilder documentId = new();
      documentId.Append(conventions.TransformTypeCollectionNameToDocumentIdPrefix(collection));
      documentId.Append(IdentityPartsSeparator);
      documentId.Append(IdGenerator.Create());

      return Task.FromResult(documentId.ToString());
    }


    /// <summary>
    /// Finds the collection name for a certain type based on internal rules
    /// </summary>
    protected virtual string FindCollectionName(Type originalType)
    {
      string collection = null;

      Type type = originalType;

      Func<string, string> cache = name =>
      {
        CachedTypeCollectionNameMap.Add(type, name);
        return name;
      };

      // get inner type for revisions
      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Revision<>))
      {
        type = type.GetGenericArguments().FirstOrDefault();
      }

      // try to resolve from cache
      if (CachedTypeCollectionNameMap.TryGetValue(type, out collection))
      {
        return collection;
      }

      // do not alter non-internal entities
      if (!AcceptsZeroConventionsType.IsAssignableFrom(type))
      {
        return cache(DocumentConventions.DefaultGetCollectionName(type));
      }

      // use name from attribute if available
      CollectionAttribute collectionAttribute = type.GetCustomAttribute<CollectionAttribute>(true);

      if (collectionAttribute != null)
      {
        return cache(collectionAttribute.Name);
      }

      // use base interface if available
      Type interfaceBaseType = type.GetInterfaces().FirstOrDefault(x => x.IsInterface && AcceptsZeroConventionsType.IsAssignableFrom(x) && x.Name != AcceptsZeroConventionsType.Name);

      if (interfaceBaseType != null)
      {
        // use name from attribute if available
        collectionAttribute = interfaceBaseType.GetCustomAttribute<CollectionAttribute>(true);
        if (collectionAttribute != null)
        {
          return cache(collectionAttribute.Name);
        }
      }

      // use base type for polymorphism
      Type polymorphBaseType = PolymorphTypes.FirstOrDefault(x => type.IsSubclassOf(x));

      if (polymorphBaseType != null)
      {
        return cache(DocumentConventions.DefaultGetCollectionName(polymorphBaseType));
      }

      return cache(DocumentConventions.DefaultGetCollectionName(type));
    }


    /// <summary>
    /// Configures the JSON serializer + deserializer for document retrieval from RavenDB
    /// </summary>
    protected virtual void ConfigureJsonSerializer(ISerializationConventions conventions)
    {
      NewtonsoftJsonSerializationConventions typedConventions = conventions as NewtonsoftJsonSerializationConventions;
      typedConventions.CustomizeJsonDeserializer = x => x.Converters.Add(new RefJsonConverter());
      typedConventions.CustomizeJsonSerializer = x => x.Converters.Add(new RefJsonConverter());
    }


    /// <summary>
    /// Translates the types collection name to the document id prefix
    /// </summary>
    protected virtual string TransformTypeCollectionNameToDocumentIdPrefix(string name)
    {
      if (!Options.Raven.CollectionPrefix.IsNullOrWhiteSpace())
      {
        name = Options.Raven.CollectionPrefix.EnsureEndsWith(IdentityPartsSeparator) + name.TrimStart(Options.Raven.CollectionPrefix);
      }

      return name.ToCamelCaseId();
    }
  }


  public interface IZeroDocumentConventionsBuilder
  {
    /// <summary>
    /// Applies internal rules to the RavenDB document conventions
    /// </summary>
    void Run(DocumentConventions conventions);
  }
}
