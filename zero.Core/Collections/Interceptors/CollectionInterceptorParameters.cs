using FluentValidation;
using Raven.Client.Documents.Session;
using System.Collections.Generic;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Collections
{
  public abstract partial class CollectionInterceptor : ICollectionInterceptor
  {
    public class Parameters<T> where T : ZeroEntity
    {
      /// <summary>
      /// The current zero context
      /// </summary>
      public IZeroContext Context { get; set; }

      /// <summary>
      /// Raven document store
      /// </summary>
      public IZeroStore Store { get; set; }

      /// <summary>
      /// Currently loaded document session
      /// </summary>
      public IAsyncDocumentSession Session { get; set; }

      /// <summary>
      /// Validator for the affected entity
      /// </summary>
      public IValidator<T> Validator { get; set; }

      /// <summary>
      /// Parameters from the interceptor which ran on before the operation (only available for completed operations)
      /// </summary>
      public Dictionary<string, object> Properties { get; set; } = new();

      /// <summary>
      /// Get a typed property
      /// </summary>
      public TProp Property<TProp>(string key) => Properties.GetValueOrDefault<TProp>(key);

      /// <summary>
      /// Get a typed property
      /// </summary>
      public bool TryGetProperty<TProp>(string key, out TProp property) => Properties.TryGetValue(key, out property);
    }


    public class ParametersWithModel<T> : Parameters<T> where T : ZeroEntity
    {
      /// <summary>
      /// The model which is affected
      /// </summary>
      public T Model { get; set; }
    }


    public class CreateParameters<T> : ParametersWithModel<T> where T : ZeroEntity
    {
    }


    public class UpdateParameters<T> : ParametersWithModel<T> where T : ZeroEntity
    {
      /// <summary>
      /// The Id of the model which is updated
      /// </summary>
      public string Id { get; set; }
    }


    public class DeleteParameters<T> : ParametersWithModel<T> where T : ZeroEntity
    {
      /// <summary>
      /// The id of the model which is deleted
      /// </summary>
      public string Id { get; set; }
    }


    public class PurgeParameters<T> : Parameters<T> where T : ZeroEntity { }
  }
}