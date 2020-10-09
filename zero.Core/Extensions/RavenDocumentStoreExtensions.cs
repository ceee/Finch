using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Client.Documents.Queries;
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
    /// <summary>
    /// Setup conventions for the document store
    /// </summary>
    public static IDocumentStore Setup(this IDocumentStore store, IZeroOptions options)
    {
      Type[] polymorphTypes = new Type[2] { typeof(SpaceContent), typeof(Page) };

      Type dbConventionType = typeof(IZeroDbConventions);

      store.Conventions.IdentityPartsSeparator = '.';

      (store.Conventions.Serialization as NewtonsoftJsonSerializationConventions).CustomizeJsonDeserializer = x =>
      {
        x.Converters.Add(new RefJsonConverter());
        x.Converters.Add(new RefsJsonConverter());
      };
      (store.Conventions.Serialization as NewtonsoftJsonSerializationConventions).CustomizeJsonSerializer = x =>
      {
        x.Converters.Add(new RefJsonConverter());
        x.Converters.Add(new RefsJsonConverter());
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


    /// <summary>
    /// Create a new unique Id
    /// </summary>
    public static string Id(this IDocumentStore store, int length = -1)
    {
      return IdGenerator.Create(length);
    }


    /// <summary>
    /// Reserves a key cluster-wide
    /// </summary>
    public static async Task<bool> ReserveAsync(this IDocumentStore store, string key, string value = null)
    {
      if (String.IsNullOrWhiteSpace(key))
      {
        return false;
      }
      if (value == null)
      {
        value = key;
      }
      var operation = new PutCompareExchangeValueOperation<string>(key, value, 0);
      CompareExchangeResult<string> result = await store.Operations.SendAsync(operation).ConfigureAwait(false);
      return result.Successful;
    }


    /// <summary>
    /// Removes a cluster-wide key reservation
    /// </summary>
    public static async Task<bool> RemoveReservationAsync(this IDocumentStore store, string key)
    {
      if (!String.IsNullOrWhiteSpace(key))
      {
        return false;
      }

      CompareExchangeValue<string> readResult = store.Operations.Send(new GetCompareExchangeValueOperation<string>(key));

      if (readResult == null)
      {
        return false;
      }

      DeleteCompareExchangeValueOperation<string> operation = new DeleteCompareExchangeValueOperation<string>(key, readResult.Index);

      CompareExchangeResult<string> result = await store.Operations.SendAsync(operation).ConfigureAwait(false);
      return result.Successful;
    }


    /// <inheritdoc />
    public static async Task PurgeAsync<T>(this IDocumentStore store, string querySuffix = null, Parameters parameters = null)
    {
      var collectionName = store.Conventions.FindCollectionName(typeof(T));

      var operationQuery = new DeleteByQueryOperation(new IndexQuery()
      {
        Query = $"from {collectionName} c {querySuffix ?? String.Empty}",
        QueryParameters = parameters
      });

      Operation operation = await store.Operations.SendAsync(operationQuery);

      await operation.WaitForCompletionAsync(TimeSpan.FromSeconds(30));
    }
  }
}
