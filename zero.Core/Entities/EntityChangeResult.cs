using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace zero.Core.Entities
{
  [DataContract(Name = "result", Namespace = "")]
  public class EntityChangeResult<T>
  {
    [DataMember(Name = "model")]
    public T Model { get; set; }

    [DataMember(Name = "success")]
    public bool IsSuccess { get; set; }

    [DataMember(Name = "errors")]
    public IList<EntityChangeResultError> Errors { get; set; } = new List<EntityChangeResultError>();

    public EntityChangeResult() { }

    public EntityChangeResult(ValidationResult validation)
    {
      IsSuccess = validation.IsValid;
      Errors = validation.Errors.Select(x => new EntityChangeResultError()
      {
        Property = x.PropertyName,
        Message = x.ErrorMessage
      }).ToList();
    }

    public void AddError(string property, string message)
    {
      IsSuccess = false;
      Errors.Add(new EntityChangeResultError()
      {
        Property = property,
        Message = message
      });
    }


    public EntityChangeResult<T> Combine(EntityChangeResult<T> with)
    {
      if (IsSuccess && !with.IsSuccess)
      {
        IsSuccess = false;
      }
      foreach (EntityChangeResultError error in with.Errors)
      {
        Errors.Add(error);
      }
      return this;
    }


    public static EntityChangeResult<T> Success() => new EntityChangeResult<T>() { IsSuccess = true };

    public static EntityChangeResult<T> Success(T model) => new EntityChangeResult<T>() { IsSuccess = true, Model = model };

    public static EntityChangeResult<T> Fail() => new EntityChangeResult<T>() { };

    public static EntityChangeResult<T> Fail(string property, string message)
    {
      EntityChangeResult<T> result = new EntityChangeResult<T>();
      result.AddError(property, message);
      return result;
    }

    public static EntityChangeResult<T> Fail(ValidationResult validation) => new EntityChangeResult<T>(validation);

    public static EntityChangeResult<T> Fail(EntityChangeResultError error) => Fail(error.Property, error.Message);
  }

  [DataContract(Name = "resultError", Namespace = "")]
  public class EntityChangeResultError
  {
    [DataMember(Name = "property")]
    public string Property { get; set; }

    [DataMember(Name = "message")]
    public string Message { get; set; }
  }
}
