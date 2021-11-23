using FluentValidation.Results;
using System.Runtime.Serialization;

namespace zero.Models;

[DataContract(Name = "result", Namespace = "")]
public class EntityResult<T>
{
  [DataMember(Name = "model")]
  public T Model { get; set; }

  [DataMember(Name = "success")]
  public bool IsSuccess { get; set; }

  [DataMember(Name = "errors")]
  public IList<EntityResultError> Errors { get; set; } = new List<EntityResultError>();

  public EntityResult() { }

  public EntityResult(ValidationResult validation)
  {
    IsSuccess = validation.IsValid;
    Errors = validation.Errors.Select(x => new EntityResultError()
    {
      Property = x.PropertyName,
      Message = x.ErrorMessage
    }).ToList();
  }

  public static EntityResult<T> From<TOrigin>(EntityResult<TOrigin> with, T model = default)
  {
    EntityResult<T> result = new EntityResult<T>();

    result.IsSuccess = with.IsSuccess;
    result.Errors = with.Errors;
    result.Model = model;

    return result;
  }

  public void AddError(string property, string message)
  {
    IsSuccess = false;
    Errors.Add(new EntityResultError()
    {
      Property = property,
      Message = message
    });
  }


  public void AddError(string message)
  {
    IsSuccess = false;
    Errors.Add(new EntityResultError()
    {
      Property = Constants.ErrorFieldNone,
      Message = message
    });
  }


  public EntityResult<T> Combine(EntityResult<T> with)
  {
    if (IsSuccess && !with.IsSuccess)
    {
      IsSuccess = false;
    }
    foreach (EntityResultError error in with.Errors)
    {
      Errors.Add(error);
    }
    return this;
  }


  public static EntityResult<T> Success() => new EntityResult<T>() { IsSuccess = true };

  public static EntityResult<T> Success(T model) => new EntityResult<T>() { IsSuccess = true, Model = model };

  public static EntityResult<T> Fail() => new EntityResult<T>() { };

  public static EntityResult<T> Fail(string property, string message)
  {
    EntityResult<T> result = new EntityResult<T>();
    result.AddError(property, message);
    return result;
  }

  public static EntityResult<T> Fail(string message)
  {
    EntityResult<T> result = new EntityResult<T>();
    result.AddError(message);
    return result;
  }

  public static EntityResult<T> Fail(ValidationResult validation) => new EntityResult<T>(validation);

  public static EntityResult<T> Fail(EntityResultError error) => Fail(error.Property, error.Message);

  public static EntityResult<T> Fail(IEnumerable<EntityResultError> errors) => new EntityResult<T>() { IsSuccess = !errors.Any(), Errors = errors.ToList() };
}


public class EntityResult : EntityResult<object>
{
  public EntityResult() : base() { }

  public EntityResult(ValidationResult validation) : base(validation) { }

  public static EntityResult Maybe(bool isSuccess) => isSuccess ? Success() : Fail();

  public new static EntityResult Success() => new EntityResult() { IsSuccess = true };

  public new static EntityResult Fail() => new EntityResult() { };
}


[DataContract(Name = "resultError", Namespace = "")]
public class EntityResultError
{
  [DataMember(Name = "property")]
  public string Property { get; set; }

  [DataMember(Name = "message")]
  public string Message { get; set; }
}
